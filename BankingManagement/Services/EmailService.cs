using System.Net;
using System.Net.Mail;

namespace BankingManagement.Services
{
    public class EmailService
    {
        private readonly string _senderEmail = "23co37@aiemgoa.ac.in";
        private readonly string _appPassword = "cupd mvpj izeu nebs";

        public void SendPasswordResetEmail(
            string receiverEmail,
            string resetLink)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(_senderEmail);
            mail.To.Add(receiverEmail);

            mail.Subject = "Banking Management - Password Reset";

            mail.Body =
                "Hello,\n\n" +
                "You requested to reset your password.\n\n" +
                "Click the link below to reset your password:\n\n" +
                resetLink +
                "\n\n" +
                "This link is valid for 30 minutes.\n\n" +
                "If you did not request a password reset, please ignore this email.\n\n" +
                "Regards,\n" +
                "Banking Management";

            mail.IsBodyHtml = false;

            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.EnableSsl = true;
                smtp.Credentials =
                    new NetworkCredential(
                        _senderEmail,
                        _appPassword
                    );

                smtp.Send(mail);
            }
        }
    }
}