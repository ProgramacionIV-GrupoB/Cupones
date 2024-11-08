using ClientesApi.Models.DTO;

namespace ClientesApi.Interfaces
{
    public interface ICuponesService
    {

        Task<string> SolicitarCupon(ClienteDto clienteDto);
        Task<string> QuemarCupon(CuponDto cuponDto);
        Task<string> GenerarNroCupon();
    }
}
