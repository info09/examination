using AdminApp.Services.Interfaces;
using Examination.Shared.Questions;
using Examination.Shared.SeedWorks;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace AdminApp.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public QuestionService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateAsync(CreateQuestionRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("MyHttpClient");
            var result = await httpClient.PostAsJsonAsync("/api/Questions", request);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var httpClient = _httpClientFactory.CreateClient("MyHttpClient");
            var result = await httpClient.DeleteAsync($"/api/Questions/{id}");
            return result.IsSuccessStatusCode;
        }

        public async Task<ApiResult<QuestionDto>> GetQuestionById(string id)
        {
            var httpClient = _httpClientFactory.CreateClient("MyHttpClient");
            var result = await httpClient.GetFromJsonAsync<ApiResult<QuestionDto>>($"/api/Questions/{id}");
            return result;
        }

        public async Task<ApiResult<PagedList<QuestionDto>>> GetQuestionsPagingAsync(QuestionSearch questionSearch)
        {
            var httpClient = _httpClientFactory.CreateClient("MyHttpClient");
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageIndex"] = questionSearch.PageNumber.ToString(),
                ["pageSize"] = questionSearch.PageSize.ToString()
            };

            if (!string.IsNullOrEmpty(questionSearch.Name))
                queryStringParam.Add("searchKeyword", questionSearch.Name);


            string url = QueryHelpers.AddQueryString("/api/Questions/paging", queryStringParam);

            var result = await httpClient.GetFromJsonAsync<ApiResult<PagedList<QuestionDto>>>(url);
            return result;
        }

        public async Task<bool> UpdateAsync(UpdateQuestionRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("MyHttpClient");
            var result = await httpClient.PutAsJsonAsync("/api/Questions", request);
            return result.IsSuccessStatusCode;
        }
    }
}
