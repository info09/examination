using AdminApp.Core;
using Blazored.SessionStorage;
using IdentityModel.Client;

namespace AdminApp.Services
{
    public class TokenService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ISessionStorageService _sessionStorage;
        private readonly ILogger<TokenService> _logger;
        private static DiscoveryDocumentResponse _disco;

        public TokenService(HttpClient httpClient, IConfiguration configuration, ISessionStorageService sessionStorageService, ILogger<TokenService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _sessionStorage = sessionStorageService;
            _logger = logger;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            // Lấy access token từ storage (localStorage hoặc sessionStorage)
            var accessToken = await _sessionStorage.GetItemAsync<string>(KeyConstants.AccessToken);
            var expiresAt = await _sessionStorage.GetItemAsync<string>(KeyConstants.ExpiresAt);
            var refreshToken = await _sessionStorage.GetItemAsync<string>(KeyConstants.RefreshToken);

            if (DateTime.UtcNow > DateTime.Parse(expiresAt))
            {
                // Access token hết hạn, gọi refresh token
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return string.Empty;
                }
                else
                {
                    var res = await RefreshTokenAsync(refreshToken);
                    return res.AccessToken;
                }
            }

            return accessToken;
        }

        private async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            _logger.LogInformation("begin RequestTokenAsync");
            _disco = await HttpClientDiscoveryExtensions.GetDiscoveryDocumentAsync(
               _httpClient,
               "http://localhost:8181/realms/exam/.well-known/openid-configuration");
            if (_disco.IsError)
            {
                throw new ApplicationException($"Status code: {_disco.IsError}, Error: {_disco.Error}");
            }
            var response = await _httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = _disco.TokenEndpoint,
                ClientId = "blazor-app",
                GrantType = "refresh_token",
                //ClientSecret = "vLeKIsvBr6F7wdaUtys6RLH607pc336J",
                ClientSecret = "jcSfbofXKyZKkZ6MyqtWh8vkSW6Gv2rn",
                Scope = "email openid roles profile offline_access",
                RefreshToken = refreshToken
            });

            if (response.IsError == false)
            {
                await _sessionStorage.SetItemAsync(KeyConstants.AccessToken, response.AccessToken);
                await _sessionStorage.SetItemAsync(KeyConstants.RefreshToken, response.RefreshToken);
                await _sessionStorage.SetItemAsync(KeyConstants.ExpiresAt, DateTime.UtcNow.AddSeconds(response.ExpiresIn));
            }
            return response;
        }
    }
}
