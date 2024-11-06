using ClientesApi.Interfaces;
using ClientesApi.Models;
using ClientesApi.Models.DTO;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace ClientesApi.Services
{
    public class ClienteService : IClienteService
    {
        public async Task<string> SolicitarCupon(ClienteDto clienteDto)
        {
            try
            { 
                var jsonCliente = JsonConvert.SerializeObject(clienteDto); // control + . (para instalar la biblioteca JsonConvert "Instalar paquete Newton...+ buscar e instalar la ult version")
                var contenido = new StringContent(jsonCliente, Encoding.UTF8, "application/json"); // guardado en un contenedor, se manda contenido al endpoint
                var cliente = new HttpClient();
                var respuesta = await cliente.PostAsync("https://localhost:7269/api/SolicitudCupones/SolicitarCupon", contenido); //(IMPORTANTE CAMBIA DEPENDIENDO LA PC LA URL)dentro del paréntesis la URL a la que le hacemos la solicitud junto con la misma 

                if (respuesta.IsSuccessStatusCode)
                {
                    var msg = await respuesta.Content.ReadAsStringAsync(); // guardar el mensaje que devuelve la api
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
                throw new Exception($"Error: {ex.Message} ");
            }
        }
    }
}
