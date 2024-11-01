using Examination.Shared.Exams;
using Examination.Shared.SeedWorks;
using Microsoft.AspNetCore.WebUtilities;
using PortalApp.Services.Interfaces;

namespace PortalApp.Services
{
    public class ExamService : BaseService, IExamService
    {
        public ExamService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, httpContextAccessor)
        {
        }

        public async Task<ApiResult<ExamDto>> GetExamByIdAsync(string id)
        {
            var result = await GetAsync<ExamDto>($"/api/Exams/{id}", true);
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

            var result = await GetAsync<PagedList<ExamDto>>(url, true);
            return result;
        }
    }
}
