using AdminApp.Core;
using AdminApp.Services.Interfaces;
using Blazored.SessionStorage;
using Examination.Shared.Categories;
using Examination.Shared.SeedWorks;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http;
using System.Net.Http.Json;

namespace AdminApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISessionStorageService _sessionStorage;

        public CategoryService(ISessionStorageService sessionStorage, IHttpClientFactory httpClientFactory)
        {   
            _sessionStorage = sessionStorage;
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task<bool> CreateAsync(CreateCategoryRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("MyHttpClient");
            var result = await httpClient.PostAsJsonAsync("/api/Categories", request);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var httpClient = _httpClientFactory.CreateClient("MyHttpClient");
            var result = await httpClient.DeleteAsync($"/api/Categories/{id}");
            return result.IsSuccessStatusCode;
        }

        public async Task<ApiResult<PagedList<CategoryDto>>> GetCategoriesPagingAsync(CategorySearch searchInput)
        {
            var httpClient = _httpClientFactory.CreateClient("MyHttpClient");
            
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageIndex"] = searchInput.PageNumber.ToString(),
                ["pageSize"] = searchInput.PageSize.ToString()
            };

            if (!string.IsNullOrEmpty(searchInput.Name))
                queryStringParam.Add("searchKeyword", searchInput.Name);

            string url = QueryHelpers.AddQueryString("/api/Categories/paging", queryStringParam);

            var result = await httpClient.GetFromJsonAsync<ApiResult<PagedList<CategoryDto>>>(url);

            return result;
        }

        public async Task<ApiResult<CategoryDto>> GetCategoryByIdAsync(string id)
        {
            var httpClient = _httpClientFactory.CreateClient("MyHttpClient");


            // Thực hiện yêu cầu GET
            var result = await httpClient.GetFromJsonAsync<ApiResult<CategoryDto>>($"/api/Categories/{id}");

            return result;
        }

        public async Task<bool> UpdateAsync(UpdateCategoryRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("MyHttpClient");
            var result = await httpClient.PutAsJsonAsync($"/api/Categories", request);
            return result.IsSuccessStatusCode;
        }

        public async Task<ApiResult<List<CategoryDto>>> GetAllCategories()
        {
            var httpClient = _httpClientFactory.CreateClient("MyHttpClient");
            var result = await httpClient.GetFromJsonAsync<ApiResult<List<CategoryDto>>>("/api/Categories");
            return result;
        }
    }
}
