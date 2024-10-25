using Examination.Shared.Questions;
using Examination.Shared.SeedWorks;
using MediatR;

namespace Examination.Application.Queries.Questions.GetQuestionById
{
    public class GetQuestionByIdQuery : IRequest<ApiResult<QuestionDto>>
    {
        public string Id { get; set; }
        public GetQuestionByIdQuery(string id)
        {
            Id = id;
        }
    }
}
