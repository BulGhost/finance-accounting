using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Services.Interfaces;
using System.Net.Http.Json;

namespace FinanceAccounting.WebUI.Services
{
    public class CategoriesClient : ICategoriesClient
    {
        private readonly HttpClient _httpClient;

        public CategoriesClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:5021/");
        }

        //public async Task<IEnumerable<CategoryDto>> GetCategories(OperationType operationType)
        //{
        //    return await _httpClient.GetFromJsonAsync<CategoryDto[]>($"api/categories/{(int)operationType}");
        //}
    }
}
