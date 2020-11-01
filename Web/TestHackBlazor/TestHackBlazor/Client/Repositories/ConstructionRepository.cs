using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TestHackBlazor.Shared.DTO;
using TestHackBlazor.Shared.Extensions;

namespace TestHackBlazor.Client.Repositories
{
    public class ConstructionRepository
    {
        private HttpClient HttpClient { get; set; }

        public ConstructionRepository(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<List<ConstructionDTO>> GetList()
        {
            var constructs = await HttpClient.GetFromJsonAsync<ConstructionDTO[]>("api/construction/list");
            return constructs.ToList();
        }

        //public async Task<bool> DeleteUserLink(long userLinkId)
        //{
        //    var result = await HttpClient.DeleteAsync($"UserLink/delete/{userLinkId}");
        //    if (result.IsSuccessStatusCode)
        //        return true;

        //    return false;
        //}

        public async Task<ConstructionDTO> Get(long constructionId)
        {
            if (constructionId < 1)
                return null;

            Console.WriteLine($"Send get construction {constructionId}");

            var result = await HttpClient.GetFromJsonAsync<ConstructionDTO>("api/construction/get/" + constructionId);
            if (result == null)
            {
                Console.WriteLine($"Send get construction {constructionId} res is null.");
                return null;
            }

            Console.WriteLine($"Send get construction {constructionId} OK.");

            return result;
        }

        public async Task<bool> Create(ConstructionCreateDTO construct)
        {
            if (construct == null || string.IsNullOrWhiteSpace(construct.Name) || construct.BorderPoints.IsNullOrEmpty())
                return false;

            var result = await HttpClient.PostAsJsonAsync($"api/construction/add", construct);
            if (result.IsSuccessStatusCode)
                return true;

            return false;
        }

        public async Task<bool> Update(ConstructionDTO construct)
        {
            if (construct == null || construct.Id <= 0)
                return false;

            var result = await HttpClient.PostAsJsonAsync($"api/construction/update", construct);
            if (result.IsSuccessStatusCode)
                return true;

            return false;
        }
    }
}
