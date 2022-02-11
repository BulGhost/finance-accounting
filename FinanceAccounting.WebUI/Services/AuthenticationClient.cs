using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models.Requests;
using FinanceAccounting.WebUI.Services.AuthProvider;
using FinanceAccounting.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace FinanceAccounting.WebUI.Services
{
    public class AuthenticationClient : IAuthenticationClient
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationClient(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:5021/api/v1/");
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<RegistrationResponseDto> RegisterUser(RegistrationRequest userForRegistrationDto)
        {
            string content = JsonSerializer.Serialize(userForRegistrationDto);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            HttpResponseMessage registrationResult = await _httpClient.PostAsync("register", bodyContent);
            Stream registrationContent = await registrationResult.Content.ReadAsStreamAsync();

            if (registrationResult.IsSuccessStatusCode)
            {
                return new RegistrationResponseDto {IsSucceeded = true, ErrorMessage = string.Empty };
            }

            JsonDocument jsonContent = await JsonDocument.ParseAsync(registrationContent);
            JsonElement errorMessage = jsonContent.RootElement.GetProperty("errorMessage");
            return new RegistrationResponseDto {IsSucceeded = false, ErrorMessage = errorMessage.GetString()};
        }

        public async Task<AuthResponseDto> Login(AuthenticationRequest userForAuthentication)
        {
            string content = JsonSerializer.Serialize(userForAuthentication);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage authResult = await _httpClient.PostAsync("login", bodyContent);
            Stream authContent = await authResult.Content.ReadAsStreamAsync();
            JsonDocument jsonContent = await JsonDocument.ParseAsync(authContent);

            if (!authResult.IsSuccessStatusCode)
            {
                JsonElement errorMessage = jsonContent.RootElement.GetProperty("errorMessage");
                return new AuthResponseDto {IsSucceeded = false, ErrorMessage = errorMessage.GetString()};
            }

            string accessToken = jsonContent.RootElement.GetProperty("accessToken").GetString();
            await _localStorage.SetItemAsync("accessToken", accessToken);
            string refreshToken = jsonContent.RootElement.GetProperty("refreshToken").GetString();
            await _localStorage.SetItemAsync("refreshToken", refreshToken);
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(userForAuthentication.UserName);
            return new AuthResponseDto {IsSucceeded = true, AccessToken = accessToken, RefreshToken = refreshToken};
        }

        public async Task<AuthResponseDto> RefreshToken()
        {
            var accessToken = await _localStorage.GetItemAsync<string>("accessToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");
            string content = JsonSerializer.Serialize(new RefreshTokenRequestDto { AccessToken = accessToken, RefreshToken = refreshToken });
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage refreshResult = await _httpClient.PostAsync("refresh-token", bodyContent);
            Stream refreshContent = await refreshResult.Content.ReadAsStreamAsync();
            JsonDocument jsonContent = await JsonDocument.ParseAsync(refreshContent);

            if (!refreshResult.IsSuccessStatusCode)
            {
                JsonElement errorMessage = jsonContent.RootElement.GetProperty("errorMessage");
                return new AuthResponseDto { IsSucceeded = false, ErrorMessage = errorMessage.GetString() };
            }

            accessToken = jsonContent.RootElement.GetProperty("accessToken").GetString();
            await _localStorage.SetItemAsync("accessToken", accessToken);
            refreshToken = jsonContent.RootElement.GetProperty("refreshToken").GetString();
            await _localStorage.SetItemAsync("refreshToken", refreshToken);
            return new AuthResponseDto { IsSucceeded = true, AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("accessToken");
            await _localStorage.RemoveItemAsync("refreshToken");
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        }
    }
}
