using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models.ReportInterval;
using FinanceAccounting.WebUI.Entities.Models.Requests;
using FinanceAccounting.WebUI.Exceptions;
using FinanceAccounting.WebUI.HelperClasses;
using FinanceAccounting.WebUI.Services.Interfaces;

namespace FinanceAccounting.WebUI.Services
{
    public class OperationsClient : IOperationsClient, IDisposable
    {
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
        private readonly Lazy<Task<HttpClient>> _configuredHttpClient;

        public OperationsClient(HttpClient httpClient, ITokenService tokenService)
        {
            _configuredHttpClient = new Lazy<Task<HttpClient>>(async () =>
            {
                httpClient.BaseAddress = new Uri("https://localhost:5021/api/v1/operations/");
                string accessToken = await tokenService.GetActualAccessToken().ConfigureAwait(false);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                return httpClient;
            });

            _options.Converters.Add(new DateTimeConverter());
        }

        public async Task<OperationsReportDto> GetOperationsReport(IReportInterval reportInterval)
        {
            if (reportInterval.StartDate > DateTime.Now)
            {
                throw new ArgumentException("Start date cannot be in the future");
            }

            DateTime startDate = reportInterval.StartDate;
            DateTime finalDate = reportInterval.FinalDate < DateTime.Now
                ? reportInterval.FinalDate
                : DateTime.Today;
            HttpClient httpClient = await _configuredHttpClient.Value;
            string requestQueryString = startDate == finalDate
                ? $"days-report?date={startDate:yyyy-MM-dd}"
                : $"period-report?startDate={startDate:yyyy-MM-dd}&finalDate={finalDate:yyyy-MM-dd}";
            HttpResponseMessage getOperationsReportResult = await httpClient.GetAsync(requestQueryString);
            string jsonContent = await getOperationsReportResult.Content.ReadAsStringAsync();

            if (getOperationsReportResult.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<OperationsReportDto>(jsonContent, _options);
            }

            throw new UnsuccessfulResponseException("Fail on getting operations report attempt");
        }

        public async Task<OperationDto> GetOperationById(int operationId)
        {
            HttpClient httpClient = await _configuredHttpClient.Value;
            HttpResponseMessage getOperationResult = await httpClient.GetAsync($"{operationId}");
            string jsonContent = await getOperationResult.Content.ReadAsStringAsync();

            if (getOperationResult.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<OperationDto>(jsonContent, _options);
            }

            throw new UnsuccessfulResponseException("Fail on getting operation attempt");
        }

        public async Task<CommandResponseDto> AddOperation(AddOperationRequest newOperation)
        {
            string content = JsonSerializer.Serialize(new List<AddOperationRequest> { newOperation }, _options);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            HttpClient httpClient = await _configuredHttpClient.Value;
            HttpResponseMessage operationAdditionResult = await httpClient.PostAsync(string.Empty, bodyContent);

            return await RetrieveHttpResponseMessageAsync(operationAdditionResult);
        }

        public async Task<CommandResponseDto> UpdateOperation(UpdateOperationRequest updatedOperation)
        {
            string content = JsonSerializer.Serialize(new List<UpdateOperationRequest> { updatedOperation }, _options);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            HttpClient httpClient = await _configuredHttpClient.Value;
            HttpResponseMessage operationUpdatingResult = await httpClient.PutAsync(string.Empty, bodyContent);

            return await RetrieveHttpResponseMessageAsync(operationUpdatingResult);
        }

        public async Task<CommandResponseDto> DeleteOperation(int operationId)
        {
            var request = new HttpRequestMessage
            {
                Content = JsonContent.Create(new List<int> { operationId }),
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
