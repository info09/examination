using AdminApp.Services.Interfaces;
using Examination.Shared.Exams;
using Examination.Shared.SeedWorks;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using System.Text.Json;

namespace AdminApp.Services
{
    public class ExamService : IExamService
    {
        private readonly HttpClient _httpClient;
        public ExamService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ApiResult<ExamDto>> CreateAsync(CreateExamRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("/api/Exams", request);
            var content = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResult<ExamDto>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<ApiResult<bool>> DeleteAsync(string id)
        {
            var result = await _httpClient.DeleteAsync($"/api/Exams/{id}");
            var content = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResult<bool>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<ApiResult<ExamDto>> GetExamByIdAsync(string id)
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResult<ExamDto>>($"/api/Exams/{id}");
            return result;
        }

        public async Task<ApiResult<PagedList<ExamDto>>> GetExamsPagingAsync(ExamSearch examSearch)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageIndex"] = examSearch.PageNumber.ToString(),
                ["pageSize"] = examSearch.PageSize.ToString()
            };
            if (!string.IsNullOrEmpty(examSearch.Name))
                queryStringParam.Add("searchKeyword", examSearch.Name);
            if (!string.IsNullOrEmpty(examSearch.CategoryId))
                queryStringParam.Add("categoryId", examSearch.CategoryId);
            string url = QueryHelpers.AddQueryString("/api/Exams/paging", queryStringParam);
            var result = await _httpClient.GetFromJsonAsync<ApiResult<PagedList<ExamDto>>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> UpdateAsync(UpdateExamRequest request)
        {
            var result = await _httpClient.PutAsJsonAsync("/api/Exams", request);
            var content = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResult<bool>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
