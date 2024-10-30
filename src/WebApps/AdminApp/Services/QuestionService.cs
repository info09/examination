using AdminApp.Services.Interfaces;
using Examination.Shared.Questions;
using Examination.Shared.SeedWorks;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace AdminApp.Services
{
    public class QuestionService : IQuestionService
    {
        private HttpClient _httpClient;
        public QuestionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateAsync(CreateQuestionRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("/api/Questions", request);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _httpClient.DeleteAsync($"/api/Questions/{id}");
            return result.IsSuccessStatusCode;
        }

        public async Task<ApiResult<QuestionDto>> GetQuestionById(string id)
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResult<QuestionDto>>($"/api/Questions/{id}");
            return result;
        }

        public async Task<ApiResult<PagedList<QuestionDto>>> GetQuestionsPagingAsync(QuestionSearch questionSearch)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageIndex"] = questionSearch.PageNumber.ToString(),
                ["pageSize"] = questionSearch.PageSize.ToString()
            };

            if (!string.IsNullOrEmpty(questionSearch.Name))
                queryStringParam.Add("searchKeyword", questionSearch.Name);


            string url = QueryHelpers.AddQueryString("/api/Questions/paging", queryStringParam);

            var result = await _httpClient.GetFromJsonAsync<ApiResult<PagedList<QuestionDto>>>(url);
            return result;
        }

        public async Task<bool> UpdateAsync(UpdateQuestionRequest request)
        {
            var result = await _httpClient.PutAsJsonAsync("/api/Questions", request);
            return result.IsSuccessStatusCode;
        }
    }
}
