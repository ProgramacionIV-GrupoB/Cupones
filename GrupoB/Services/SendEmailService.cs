using CuponesApi.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CuponesApi.Services
{
    public class SendEmailService : ISendEmailService
    {
        private readonly string _emailFrom = "recursoshumanos.noresponder@gmail.com";
        private readonly string _emailPassword = "drrj ablq dcul yjxt";
        private readonly string _smtpServer = "smtp.gmail.com";

        public async Task EnviarEmailReclamo(string emailCliente, string Id_Cupon)
        {
            await EnviarEmail(emailCliente, "Número de cupón asignado", $"Su número de cupón es: {Id_Cupon}.");
        }

        public async Task EnviarEmailUso(string emailCliente, string nroCupon)
        {
            await EnviarEmail(emailCliente, "Cupón utilizado", $"Su cupón con el número {nroCupon} ha sido utilizado.");
        }

        private async Task EnviarEmail(string emailTo, string subject, string body)
        {
            try
            {
                using (var smtpClient = new SmtpClient(_smtpServer))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential(_emailFrom, _emailPassword);
                    smtpClient.EnableSsl = true;

                    var message = new MailMessage
                    {
                        From = new MailAddress(_emailFrom, "ProgramacionIV"),
                        To = { new MailAddress(emailTo) },
                        Subject = subject,
                        Body = body
                    };

                    await smtpClient.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}