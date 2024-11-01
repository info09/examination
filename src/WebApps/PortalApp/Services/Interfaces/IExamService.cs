using Examination.Shared.Exams;
using Examination.Shared.SeedWorks;

namespace PortalApp.Services.Interfaces
{
    public interface IExamService
    {
        Task<ApiResult<PagedList<ExamDto>>> GetExamsPagingAsync(ExamSearch examSearch);
        Task<ApiResult<ExamDto>> GetExamByIdAsync(string id);
    }
}
