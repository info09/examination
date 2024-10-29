using AdminApp.Core;
using AdminApp.Services.Interfaces;
using Blazored.SessionStorage;
using Examination.Shared.Categories;
using Examination.Shared.SeedWorks;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace AdminApp.Services
{
    public class CategoryService : ICategoryService
    {
        public HttpClient _httpClient;
        private readonly ISessionStorageService _sessionStorage;

        public CategoryService(HttpClient httpClient, ISessionStorageService sessionStorage)
        {   
            _httpClient = httpClient;
            _sessionStorage = sessionStorage;
        }
        
        public async Task<bool> CreateAsync(CreateCategoryRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("/api/Categories", request);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _httpClient.DeleteAsync($"/api/Categories/{id}");
            return result.IsSuccessStatusCode;
        }

        public async Task<ApiResult<PagedList<CategoryDto>>> GetCategoriesPagingAsync(CategorySearch searchInput)
        {
            var token = await _sessionStorage.GetItemAsStringAsync(KeyConstants.AccessToken);
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Trim('"'));
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageIndex"] = searchInput.PageNumber.ToString(),
                ["pageSize"] = searchInput.PageSize.ToString()
            };

            if (!string.IsNullOrEmpty(searchInput.Name))
                queryStringParam.Add("searchKeyword", searchInput.Name);

            string url = QueryHelpers.AddQueryString("/api/Categories/paging", queryStringParam);

            var result = await _httpClient.GetFromJsonAsync<ApiResult<PagedList<CategoryDto>>>(url);

            return result;
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(string id)
        {
            var result = await _httpClient.GetFromJsonAsync<CategoryDto>($"/api/Categories/{id}");
            return result!;
        }

        public async Task<bool> UpdateAsync(UpdateCategoryRequest request)
        {
            var result = await _httpClient.PatchAsJsonAsync($"/api/Categories", request);
            return result.IsSuccessStatusCode;
        }
    }
}
