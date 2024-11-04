using AdminApp.Services.Interfaces;
using System.Net.Http.Headers;
using System.Net;

namespace AdminApp.Services
{
    public class TokenAuthenticationHandler : DelegatingHandler
    {
        private readonly TokenService _tokenService;

        public TokenAuthenticationHandler(TokenService tokenService)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenService.GetAccessTokenAsync();
            Console.WriteLine($"Token: {token}"); // Debug token
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await base.SendAsync(request, cancellationToken);



            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Thử lại với access token mới nếu có lỗi 401
                token = await _tokenService.GetAccessTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    response = await base.SendAsync(request, cancellationToken);
                }
            }

            return response;
        }
    }
}
