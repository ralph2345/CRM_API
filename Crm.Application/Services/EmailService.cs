using System.Net;
using System.Net.Mail;
using Crm.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Crm.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendPasswordResetEmail(string email)
        {
            try
            {
                using (var smtpClient = new SmtpClient(_configuration["Email:SmtpServer"]))
                {
                    smtpClient.Port = int.Parse(_configuration["Email:SmtpPort"]);
                    smtpClient.Credentials = new NetworkCredential(
                        _configuration["Email:Username"],
                        _configuration["Email:Password"]
                    );
                    smtpClient.EnableSsl = true;


                    //convert email to byte array
                    byte[] emailBytes = System.Text.Encoding.UTF8.GetBytes(email);

                    //encrypt email to base64 string
                    string encryptedEmail = Convert.ToBase64String(emailBytes);

                    //frontend url
                    string resetUrl = $"http://192.168.1.27:8000/reset-password/{encryptedEmail}";

                    // Email Content
                    string subject = "Password Reset Request";
                    string body = $@"
                        <h2>Password Reset Request</h2>
                        <p>Click the link below to reset your password:</p>
                        <p><a href='{resetUrl}' target='_blank'><strong>Reset Your Password</strong></a></p>
                        <p>If you didn't request this, you can safely ignore this email.</p>
                        <p>Thank you,<br><strong>CRM Support</strong></p>";

                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(_configuration["Email:FromAddress"]);
                        mailMessage.To.Add(email);
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = true; // Enable HTML formatting

                        // Send Email
                        await smtpClient.SendMailAsync(mailMessage);

                    }
                }
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error: {smtpEx.StatusCode} - {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
