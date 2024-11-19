public class EmailMessage
{
    public int Id { get; set; }
    public required string FromEmail { get; set; }  // ต้องไม่เป็น null
    public required string ToEmail { get; set; }    // ต้องไม่เป็น null
    public string Subject { get; set; }
    public string Message { get; set; }
    public DateTime SentDate { get; set; }
    public bool IsRead { get; set; }

    public EmailMessage(string fromEmail, string toEmail, string subject, string message, DateTime sentDate, bool isRead)
    {
        FromEmail = fromEmail;
        ToEmail = toEmail;
        Subject = subject;
        Message = message;
        SentDate = sentDate;
        IsRead = isRead;
    }
}
