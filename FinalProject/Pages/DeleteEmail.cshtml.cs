using FinalProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace FinalProject.Pages
{
    public class DeleteEmailModel : PageModel
    {
        private readonly ILogger<DeleteEmailModel> _logger;

        public DeleteEmailModel(ILogger<DeleteEmailModel> logger)
        {
            _logger = logger;
        }

        // The email ID to delete
        public int EmailID { get; set; }

        // OnGet method to receive emailid and display the email page
        public IActionResult OnGet(int emailid)
        {
            try
            {
                // Ensure emailid is valid
                if (emailid <= 0)
                {
                    _logger.LogWarning("Invalid EmailID passed to OnGet method.");
                    return BadRequest("Invalid EmailID.");
                }

                EmailID = emailid;
                return Page(); // Load the page with the email ID
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching email details: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // OnPost method to delete the email from the database
        public IActionResult OnPost(int emailid)
        {
            try
            {
                // Ensure emailid is valid
                if (emailid <= 0)
                {
                    _logger.LogWarning("Invalid EmailID passed to OnPost method.");
                    return BadRequest("Invalid EmailID.");
                }

                // Connection string
                string connectionString = "Server=tcp:dekcom1234.database.windows.net,1433;Initial Catalog=Dekcom;Persist Security Info=False;User ID=dekcom;Password=ABMN_1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "DELETE FROM emails WHERE id = @EmailID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@EmailID", emailid);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            _logger.LogInformation($"Email with ID {emailid} deleted successfully.");
                            return RedirectToPage("/Index"); // Redirect back to inbox
                        }
                        else
                        {
                            _logger.LogWarning($"No email found with ID {emailid}");
                            return NotFound(); // Return 404 if email not found
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError($"SQL Error: {sqlEx.Message}");
                return StatusCode(500, "Database error");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting email: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}