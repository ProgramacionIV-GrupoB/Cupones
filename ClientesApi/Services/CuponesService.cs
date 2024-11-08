
using ClientesApi.Interfaces;
using ClientesApi.Models.DTO;
using Newtonsoft.Json;
using System.Text;

namespace ClientesApi.Services
{
    public class CuponesService : ICuponesService
    {
        public async Task<string> SolicitarCupon(ClienteDto clienteDTO)
        {
            try
            {
                var jsonCliente = JsonConvert.SerializeObject(clienteDTO);
                var contenido = new StringContent(jsonCliente, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                var respuesta = await client.PostAsync("https://localhost:7269/api/SolicitudCupones/SolicitarCupon", contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    var msg = await respuesta.Content.ReadAsStringAsync();
                    return msg;
                }
                else
                {
                    var error = await respuesta.Content.ReadAsStringAsync();
                    throw new Exception($"{error}");
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task<string> QuemarCupon(CuponDto cuponDto)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(cuponDto);
                var contenido = new StringContent(jsonData, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var respuesta = await client.PostAsync("https://localhost:7269/api/SolicitudCupones/QuemadoCupon", contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    var message = await respuesta.Content.ReadAsStringAsync();
                    return message;
                }
                else
                {
                    var error = await respuesta.Content.ReadAsStringAsync();
                    throw new Exception($"{error}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task<string> GenerarNroCupon()
        {

            Random random = new Random();
            string nroCupon = "";

            for (int i = 0; i < 9; i++)
            {
                nroCupon += random.Next(0, 10).ToString();

                if (i == 2 || i == 5)
                {
                    nroCupon += "-";
                }
            }

            return nroCupon;
        }
    }
}
