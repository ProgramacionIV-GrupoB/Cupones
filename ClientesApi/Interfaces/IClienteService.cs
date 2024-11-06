using ClientesApi.Models.DTO;

namespace ClientesApi.Interfaces
{
    public interface IClienteService
    {
        Task<string> SolicitarCupon(ClienteDto clienteDto);
    }
}
