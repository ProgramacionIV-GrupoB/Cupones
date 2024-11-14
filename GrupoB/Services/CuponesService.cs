using CuponesApi.Interfaces;

namespace CuponesApi.Services
{
    public class CuponesService : ICuponesService
    {
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
