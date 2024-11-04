using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AdminApp.Services
{
    public class TokenService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;
        private static DiscoveryDocumentResponse _disco;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(HttpClient httpClient, IConfiguration configuration, ILogger<TokenService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            // Lấy access token từ storage (localStorage hoặc sessionStorage)
            var accessToken = await _httpContextAccessor?.HttpContext?.GetTokenAsync("access_token");
            var refreshToken = await _httpContextAccessor?.HttpContext?.GetTokenAsync("refresh_token");
            var expiresAt = await _httpContextAccessor?.HttpContext?.GetTokenAsync("expires_at");

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
                ClientId = _configuration["KeyCloak:ClientId"],
                GrantType = "refresh_token",
                ClientSecret = _configuration["KeyCloak:ClientSecret"],
                Scope = "email openid roles profile offline_access",
                RefreshToken = refreshToken
            });
            var expiresAt = DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn);
            var authProperties = new AuthenticationProperties();
            authProperties.StoreTokens(new[]
            {
                new AuthenticationToken { Name = "access_token", Value = response.AccessToken },
                new AuthenticationToken { Name = "refresh_token", Value = response.RefreshToken },
                new AuthenticationToken { Name = "expires_at", Value = expiresAt.ToString("o") }
            });

            var authResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authResult.Principal, authProperties);

            return response;
        }
    }
}
