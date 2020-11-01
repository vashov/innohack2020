using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TestHackBlazor.Shared.DTO;

namespace TestHackBlazor.Client.Repositories
{
    public class UserRepository
    {
        private HttpClient HttpClient { get; set; }

        public UserRepository(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<List<UserBaseInfoDTO>> GetList()
        {
            var users = await HttpClient.GetFromJsonAsync<UserBaseInfoDTO[]>("api/user/list-base-info");
            return users.ToList();
        }
    }
}
