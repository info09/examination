using AdminApp.Services;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Headers;

namespace AdminApp.Core.Authentication
{
    public class TokenAuthenticationHandler : DelegatingHandler
    {
        private readonly TokenService _tokenService;
        private readonly NavigationManager _navigationManager;

        public TokenAuthenticationHandler(TokenService tokenService, NavigationManager navigationManager)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
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
                else
                {
                    _navigationManager.NavigateTo(UrlConstants.Login);
                }
            }

            return response;
        }
    }
}
