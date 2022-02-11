using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Services.Interfaces;
using System.Text;
using System.Text.Json;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Enums;
using FinanceAccounting.WebUI.Entities.Models.Requests;
using FinanceAccounting.WebUI.Exceptions;

namespace FinanceAccounting.WebUI.Services
{
    public class CategoriesClient : ICategoriesClient, IDisposable
    {
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
        private readonly Lazy<Task<HttpClient>> _configuredHttpClient;

        public CategoriesClient(HttpClient httpClient, TokenService tokenService)
        {
            _configuredHttpClient = new Lazy<Task<HttpClient>>(async () =>
            {
                httpClient.BaseAddress = new Uri("https://localhost:5021/api/v1/categories/");
                string accessToken = await tokenService.GetActualAccessToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                return httpClient;
            });
        }

        public async Task<List<CategoryDto>> GetAllCategories()
        {
            HttpClient httpClient = await _configuredHttpClient.Value;
            var categoryList = new List<CategoryDto>();
            var incomeCategories = await GetCategoriesOfType(OperationType.Income, httpClient);
            categoryList.AddRange(incomeCategories);
            var expenseCategories = await GetCategoriesOfType(OperationType.Expense, httpClient);
            categoryList.AddRange(expenseCategories);
            return categoryList;
        }

        public async Task<CategoryDto> GetCategoryById(int categoryId)
        {
            HttpClient httpClient = await _configuredHttpClient.Value;
            HttpResponseMessage getCategoryResult = await httpClient.GetAsync($"find-by-id/{categoryId}");
            string jsonContent = await getCategoryResult.Content.ReadAsStringAsync();

            if (getCategoryResult.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<CategoryDto>(jsonContent, _options);
            }

            throw new UnsuccessfulResponseException("Fail on getting category attempt");
        }

        public async Task<CommandResponseDto> CreateCategory(CreateCategoryRequest newCategory)
        {
            string content = JsonSerializer.Serialize(new List<CreateCategoryRequest> {newCategory});
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            HttpClient httpClient = await _configuredHttpClient.Value;
            HttpResponseMessage categoryCreationResult = await httpClient.PostAsync(string.Empty, bodyContent);

            return await RetrieveHttpResponseMessageAsync(categoryCreationResult);
        }

        public async Task<CommandResponseDto> UpdateCategory(UpdateCategoryRequest updatedCategory)
        {
            string content = JsonSerializer.Serialize(new List<UpdateCategoryRequest> {updatedCategory});
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            HttpClient httpClient = await _configuredHttpClient.Value;
            HttpResponseMessage categoryUpdatingResult = await httpClient.PutAsync(string.Empty, bodyContent);

            return await RetrieveHttpResponseMessageAsync(categoryUpdatingResult);
        }

        public async Task<CommandResponseDto> DeleteCategory(int categoryId)
        {
            var request = new HttpRequestMessage
            {
                Content = JsonContent.Create(new List<int> { categoryId }),
                Method = HttpMethod.Delete
            };
            HttpClient httpClient = await _configuredHttpClient.Value;
            HttpResponseMessage categoryDeletingResult = await httpClient.SendAsync(request);

            return await RetrieveHttpResponseMessageAsync(categoryDeletingResult);
        }

        public void Dispose()
        {
            if (_configuredHttpClient.IsValueCreated)
            {
                _configuredHttpClient.Value.Dispose();
            }
        }

        private async Task<List<CategoryDto>> GetCategoriesOfType(OperationType operationType, HttpClient httpClient)
        {
            HttpResponseMessage getCategoriesResult = await httpClient.GetAsync($"{(int)operationType}");
            string jsonContent = await getCategoriesResult.Content.ReadAsStringAsync();

            if (getCategoriesResult.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<List<CategoryDto>>(jsonContent, _options);
            }

            throw new UnsuccessfulResponseException("Fail on getting categories attempt");
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