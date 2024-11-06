using GrupoB.Interfaces;
using System.Net;
using System.Net.Mail;

namespace GrupoB.Services
{
    public class SendEmailService : ISendEmailService //hereda la interfaz
    {
        public async Task EnviarEmailCliente(string emailCliente, string nroCupon)
        {
            string emailDesde = "recursoshumanos.noresponder@gmail.com";
            string emailClave = "drrj ablq dcul yjxt"; //para auntenticar el envio y llevarlo a cabo, para generarlo lo explica en 1:27:00
            string servicioGoogle = "smtp.gmail.com";

            try
            {
                SmtpClient smtpClient = new SmtpClient(servicioGoogle);
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(emailDesde, emailClave);
                smtpClient.EnableSsl = true;

                MailMessage message = new MailMessage();
                message.From = new MailAddress(emailDesde, "ProgramacionIV"); // lo segundo es para que no se vea el mail al enviarlo
                message.To.Add(emailCliente);
                message.Subject = "Número de cupón asignado"; //asunto del email
                message.Body = $"Su número de cupón es: {nroCupon}."; // relaciones entre las tablas para enviarle los datos (hacer?)
                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
