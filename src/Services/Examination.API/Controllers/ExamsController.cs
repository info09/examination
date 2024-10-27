using Examination.Application.Commands.Exams.CreateExam;
using Examination.Application.Commands.Exams.DeleteExam;
using Examination.Application.Commands.Exams.UpdateExam;
using Examination.Application.Queries.Exams.GetAllExams;
using Examination.Application.Queries.Exams.GetExamById;
using Examination.Application.Queries.Exams.GetExamsPaging;
using Examination.Shared.Exams;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Examination.API.Controllers
{
    public class ExamsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ExamsController> _logger;

        public ExamsController(IMediator mediator, ILogger<ExamsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExams()
        {
            var result = await _mediator.Send(new GetAllExamsQuery());
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllExamsPaging([FromQuery] GetExamsPagingQuery query)
        {
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExamById(string id)
        {
            var result = await _mediator.Send(new GetExamByIdQuery(id));
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExamAsync([FromBody] CreateExamRequest request)
        {
            var command = new CreateExamCommand()
            {
                Name = request.Name,
                AutoGenerateQuestion = request.AutoGenerateQuestion,
                CategoryId = request.CategoryId,
                Content = request.Content,
                Duration = request.Duration,
                IsTimeRestricted = request.IsTimeRestricted,
                Level = request.Level,
                NumberOfQuestionCorrectForPass = request.NumberOfQuestionCorrectForPass,
                NumberOfQuestions = request.NumberOfQuestions,
                Questions = request.Questions,
                ShortDesc = request.ShortDesc,
            };
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExamAsync([FromBody] UpdateExamRequest request)
        {
            var command = new UpdateExamCommand()
            {
                Id = request.Id,
                Name = request.Name,
                AutoGenerateQuestion = request.AutoGenerateQuestion,
                CategoryId = request.CategoryId,
                Content = request.Content,
                Duration = request.Duration,
                IsTimeRestricted = request.IsTimeRestricted,
                Level = request.Level,
                NumberOfQuestionCorrectForPass = request.NumberOfQuestionCorrectForPass,
                NumberOfQuestions = request.NumberOfQuestions,
                Questions = request.Questions,
                ShortDesc = request.ShortDesc,
            };
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamAsync(string id)
        {
            var result = await _mediator.Send(new DeleteExamCommand(id));
            return StatusCode(result.StatusCode, result);
        }
    }
}
