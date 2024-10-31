using Examination.Shared.Exams;
using Examination.Shared.SeedWorks;

namespace AdminApp.Services.Interfaces
{
    public interface IExamService
    {
        Task<ApiResult<PagedList<ExamDto>>> GetExamsPagingAsync(ExamSearch examSearch);
        Task<ApiResult<ExamDto>> GetExamByIdAsync(string id);
        Task<ApiResult<ExamDto>> CreateAsync(CreateExamRequest request);
        Task<ApiResult<bool>> UpdateAsync(UpdateExamRequest request);
        Task<ApiResult<bool>> DeleteAsync(string id);
    }
}
