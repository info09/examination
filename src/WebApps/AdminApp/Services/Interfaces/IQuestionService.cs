using Examination.Shared.Questions;
using Examination.Shared.SeedWorks;

namespace AdminApp.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<ApiResult<PagedList<QuestionDto>>> GetQuestionsPagingAsync(QuestionSearch questionSearch);
        Task<ApiResult<QuestionDto>> GetQuestionById(string id);
        Task<bool> CreateAsync(CreateQuestionRequest request);
        Task<bool> UpdateAsync(UpdateQuestionRequest request);
        Task<bool> DeleteAsync(string id);
    }
}
