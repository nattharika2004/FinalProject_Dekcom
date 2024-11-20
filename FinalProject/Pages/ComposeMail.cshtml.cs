using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;

namespace FinalProject.Pages
{
    public class ComposeMailModel : PageModel
    {
        private readonly ILogger<ComposeMailModel> _logger;

        public ComposeMailModel(ILogger<ComposeMailModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public string Receiver { get; set; }

        [BindProperty]
        public string Subject { get; set; }

        [BindProperty]
        public string Message { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // ตรวจสอบว่า ModelState Valid ก่อน
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // ตรวจสอบว่าผู้ส่งเข้าสู่ระบบหรือไม่
                string sender = User?.Identity?.Name;
                if (string.IsNullOrEmpty(sender))
                {
                    ModelState.AddModelError(string.Empty, "You must be logged in to send an email.");
                    return Page();
                }

                // Connection String ควรแยกเก็บไว้ใน Configuration หรือ Environment Variables
                string connectionString = "Server=tcp:dekcom1234.database.windows.net,1433;Initial Catalog=Dekcom;Persist Security Info=False;User ID=dekcom;Password=ABMN_1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // คำสั่ง SQL สำหรับเพิ่มข้อมูล
                    string sql = "INSERT INTO emails (EmailSubject, EmailMessage, EmailDate, EmailIsRead, EmailSender, EmailReceiver) " +
                                 "VALUES (@Subject, @Message, @Date, @IsRead, @Sender, @Receiver)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Subject", string.IsNullOrEmpty(Subject) ? "No Subject" : Subject);
                        command.Parameters.AddWithValue("@Message", string.IsNullOrEmpty(Message) ? "No Message" : Message);
                        command.Parameters.AddWithValue("@Date", DateTime.Now);
                        command.Parameters.AddWithValue("@IsRead", 0); // 0 หมายถึงยังไม่ได้อ่าน
                        command.Parameters.AddWithValue("@Sender", sender);
                        command.Parameters.AddWithValue("@Receiver", string.IsNullOrEmpty(Receiver) ? "Unknown" : Receiver);

                        // Execute SQL Command
                        command.ExecuteNonQuery();
                    }
                }

                // Redirect ไปที่หน้า Index หลังจากบันทึกสำเร็จ
                return RedirectToPage("/Index");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error while sending email");
                ModelState.AddModelError(string.Empty, "There was a database error. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while sending email");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
            }

            // หากเกิดข้อผิดพลาด กลับไปยังหน้า ComposeMail พร้อมข้อความแสดงข้อผิดพลาด
            return Page();
        }
    }
}
