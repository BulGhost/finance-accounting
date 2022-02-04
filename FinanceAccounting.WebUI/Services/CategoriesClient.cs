using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Services.Interfaces;
using System.Text;
using System.Text.Json;
using FinanceAccounting.WebUI.Entities;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models;
using FinanceAccounting.WebUI.Exceptions;

namespace FinanceAccounting.WebUI.Services
{
    public class CategoriesClient : ICategoriesClient
    {
        private readonly HttpClient _httpClient;

        public CategoriesClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:5021/api/v1/categories/");
        }

        public async Task<List<CategoryDto>> GetCategories(OperationType operationType)
        {
            HttpResponseMessage getCategoriesResult = await _httpClient.GetAsync($"{(int)operationType}");
            string jsonContent = await getCategoriesResult.Content.ReadAsStringAsync();

            if (getCategoriesResult.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<List<CategoryDto>>(jsonContent);
            }

            throw new UnsuccessfulResponseException("Fail on getting categories attempt");
        }

        public async Task<CategoryDto> GetCategoryById(int categoryId)
        {
            HttpResponseMessage getCategoriesResult = await _httpClient.GetAsync($"find-by-id/{categoryId}");
            string jsonContent = await getCategoriesResult.Content.ReadAsStringAsync();

            if (getCategoriesResult.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<CategoryDto>(jsonContent);
            }

            throw new UnsuccessfulResponseException("Fail on getting category attempt");
        }

        public async Task<CommandResponseDto> CreateCategory(CreateCategoryRequest newCategory)
        {
            string content = JsonSerializer.Serialize(new List<CreateCategoryRequest> {newCategory});
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            HttpResponseMessage categoryCreationResult = await _httpClient.PostAsync(string.Empty, bodyContent);

            return await RetrieveHttpResponseMessageAsync(categoryCreationResult);
        }

        public async Task<CommandResponseDto> UpdateCategory(UpdateCategoryRequest updatedCategory)
        {
            string content = JsonSerializer.Serialize(new List<UpdateCategoryRequest> {updatedCategory});
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            HttpResponseMessage categoryUpdatingResult = await _httpClient.PutAsync(string.Empty, bodyContent);

            return await RetrieveHttpResponseMessageAsync(categoryUpdatingResult);
        }

        public async Task<CommandResponseDto> DeleteCategory(int categoryId)
        {
            var request = new HttpRequestMessage
            {
                Content = JsonContent.Create(new List<int> { categoryId }),
                Method = HttpMethod.Delete
            };
            HttpResponseMessage categoryDeletingResult = await _httpClient.SendAsync(request);

            return await RetrieveHttpResponseMessageAsync(categoryDeletingResult);
        }

        private async Task<CommandResponseDto> RetrieveHttpResponseMessageAsync(HttpResponseMessage responseMessage)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                return new CommandResponseDto { IsSucceeded = true, ErrorMessage = string.Empty };
            }

            Stream resultContent = await responseMessage.Content.ReadAsStreamAsync();
            JsonDocument jsonContent = await JsonDocument.ParseAsync(resultContent);
            JsonElement errorMessage = jsonContent.RootElement.GetProperty("errorMessage");
            return new CommandResponseDto { IsSucceeded = false, ErrorMessage = errorMessage.GetString() };
        }
    }
}