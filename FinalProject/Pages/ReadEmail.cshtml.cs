using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FinalProject.Pages
{
    public class ReadEmailModel : PageModel
    {
        private readonly ILogger<ReadEmailModel> _logger;
        public string EmailID { get; set; }
        public string EmailSubject { get; set; }
        public string EmailMessage { get; set; }
        public string EmailDate { get; set; }
        public string EmailSender { get; set; }

        public ReadEmailModel(ILogger<ReadEmailModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(int emailid)
        {
            try
            {
                string connectionString = "Server=tcp:dekcom.database.windows.net,1433;Initial Catalog=Dekcom2004;Persist Security Info=False;User ID=ABMN;Password=Dekcom12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // ?????????????????????????????
                    string updateSql = "UPDATE emails SET EmailIsRead = 1 WHERE ID = @EmailID";
                    using (SqlCommand updateCommand = new SqlCommand(updateSql, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@EmailID", emailid);
                        updateCommand.ExecuteNonQuery();  // ??????????????????
                    }

                    // ??????????????
                    string sql = "SELECT * FROM emails WHERE ID = @EmailID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@EmailID", emailid);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                EmailID = reader.GetInt32(0).ToString();
                                EmailSubject = reader.GetString(5);
                                EmailMessage = reader.GetString(6);
                                EmailDate = reader.GetDateTime(7).ToString();
                                EmailSender = reader.GetString(9);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading email: " + ex.Message);
            }
        }

    }
}
