using Examination.Domain.SeedWork;
using Examination.Shared.Enums;
using Examination.Shared.SeedWorks;

namespace Examination.Domain.AggregateModels.QuestionAggregate
{
    public interface IQuestionRepository : IRepositoryBase<Question>
    {
        Task<PagedList<Question>> GetQuestionsPagingAsync(string categoryId, string searchKeyword, int pageIndex, int pageSize);

        Task<Question> GetQuestionsByIdAsync(string id);

        Task<List<Question>> GetRandomQuestionsForExamAsync(string categoryId, Level level, int numberOfQuestions);
    }
}
