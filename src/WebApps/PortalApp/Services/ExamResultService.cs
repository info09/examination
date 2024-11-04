using Examination.Shared.ExamResults;
using Examination.Shared.SeedWorks;
using PortalApp.Services.Interfaces;

namespace PortalApp.Services
{
    public class ExamResultService : BaseService, IExamResultService
    {
        public ExamResultService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, httpContextAccessor)
        {
        }

        public async Task<ApiResult<ExamResultDto>> FinishExamAsync(FinishExamRequest request)
        {
            return await PostAsync<FinishExamRequest, ExamResultDto>("/api/ExamResults/finish", request, true);

        }

        public async Task<ApiResult<ExamResultDto>> GetExamResultByIdAsync(string id)
        {
            return await GetAsync<ExamResultDto>($"/api/ExamResults/{id}", true);
        }

        public async Task<ApiResult<ExamResultDto>> NextQuestionAsync(NextQuestionRequest request)
        {
            return await PostAsync<NextQuestionRequest, ExamResultDto>("/api/ExamResults/next-question", request, true);
        }

        public async Task<ApiResult<bool>> SkipExamAsync(SkipExamRequest request)
        {
            return await PutAsync<SkipExamRequest, bool>("/api/ExamResults/skip", request, true);
        }

        public async Task<ApiResult<ExamResultDto>> StartExamAsync(StartExamRequest request)
        {
            return await PostAsync<StartExamRequest, ExamResultDto>("/api/ExamResults/start", request, true);
        }
    }
}
