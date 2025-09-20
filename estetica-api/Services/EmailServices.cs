using SendGrid;
using SendGrid.Helpers.Mail;

namespace dentist_panel_api.Services;

public class EmailServices
{
    private readonly IConfiguration _configuration;
    public static readonly bool IsTest = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName.ToLowerInvariant().Contains("mvc.testing"));
    public static readonly string ApiKey = Environment.GetEnvironmentVariable("DENTIST_SENDGRID_APIKEY");
    
    public EmailServices(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<Response> SendHtmlEmail(string toEmail, string toName, string subject, string htmlContent)
    {
        if (IsTest || toEmail is null || toEmail.Equals("")) return null;
        var client = new SendGridClient(ApiKey);
        var from = new EmailAddress(_configuration.GetValue<string>("Sendgrid:SenderEmail"), _configuration.GetValue<string>("Sendgrid:SenderName"));
        var to = new EmailAddress(toEmail, toName);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, htmlContent, htmlContent);
        return await client.SendEmailAsync(msg);
    }
}