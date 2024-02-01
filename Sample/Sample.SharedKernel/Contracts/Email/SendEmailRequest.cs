namespace Sample.SharedKernel.Contracts.Email;

public sealed class SendEmailRequest
{
    public SendEmailRequest(string emailTo, string subject, string body)
    {
        EmailTo = emailTo;
        Subject = subject;
        Body = body;
    }
    
    public string EmailTo { get; }

    public string Subject { get; }

    public string Body { get; }
}