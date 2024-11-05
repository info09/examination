using System.Net.Http.Headers;
using System.Net;
using AdminApp.Services;
using Microsoft.AspNetCore.Components;

namespace PortalApp.Extensions
{
    public class TokenAuthenticationHandler : DelegatingHandler
    {
        private readonly TokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenAuthenticationHandler(TokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
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
                    _httpContextAccessor.HttpContext.Response.Redirect("/login.html");
                }
            }

            return response;
        }
    }
}
