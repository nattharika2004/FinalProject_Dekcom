using FinalProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Numerics;

namespace FinalProject.Pages
{
    public class IndexModel : PageModel
    {
        public List<EmailInfo> listEmails = new List<EmailInfo>();

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            try
            {
                String connectionString = "Server=tcp:dekcom1234.database.windows.net,1433;Initial Catalog=Dekcom;Persist Security Info=False;User ID=dekcom;Password=ABMN_1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string username = User.Identity.Name ?? ""; // Default to empty string if no username is found

                    String sql = "SELECT * FROM emails WHERE emailreceiver=@username";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmailInfo emailInfo = new EmailInfo
                                {
                                    EmailID = reader.IsDBNull(0) ? "" : reader.GetInt32(0).ToString(),
                                    EmailSubject = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                    EmailMessage = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                    EmailDate = reader.IsDBNull(7) ? "" : reader.GetDateTime(7).ToString(),
                                    EmailIsRead = reader.IsDBNull(8) ? "0" : reader.GetInt32(8).ToString(),
                                    EmailSender = reader.IsDBNull(9) ? "" : reader.GetString(9),
                                    EmailReceiver = reader.IsDBNull(10) ? "" : reader.GetString(10)
                                };

                                listEmails.Add(emailInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching emails: " + ex.Message);
            }
        }
    }

    public class EmailInfo
    {
        public string EmailID { get; set; }
        public string EmailSubject { get; set; }
        public string EmailMessage { get; set; }
        public string EmailDate { get; set; }
        public string EmailIsRead { get; set; }
        public string EmailSender { get; set; }
        public string EmailReceiver { get; set; }
    }
}