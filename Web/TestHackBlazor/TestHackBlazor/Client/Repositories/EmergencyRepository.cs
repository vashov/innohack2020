using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TestHackBlazor.Shared.DTO;

namespace TestHackBlazor.Client.Repositories
{
    public class EmergencyRepository
    {
        private HttpClient HttpClient { get; set; }

        public EmergencyRepository(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<List<EmergencyDTO>> GetList()
        {
            var emergs = await HttpClient.GetFromJsonAsync<EmergencyDTO[]>("api/emergency/list-all");
            return emergs.ToList();
        }

        public async Task<EmergencyDTO> Get(long emergencyId)
        {
            var emergs = await HttpClient.GetFromJsonAsync<EmergencyDTO>("api/emergency/get/" + emergencyId);
            return emergs;
        }
    }
}
