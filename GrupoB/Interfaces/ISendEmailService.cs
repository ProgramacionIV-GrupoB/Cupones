namespace CuponesApi.Interfaces
{
    public interface ISendEmailService
    {
        Task EnviarEmailReclamo(string emailCliente, string nroCupon);
        Task EnviarEmailUso(string emailCliente, string nroCupon);
    }
}