using Examination.Application.Commands.ExamResults.FinishExam;
using Examination.Application.Commands.ExamResults.SkipExam;
using Examination.Application.Commands.ExamResults.StartExam;
using Examination.Application.Commands.ExamResults.SubmitQuestion;
using Examination.Application.Queries.ExamResults.GetExamResultById;
using Examination.Application.Queries.ExamResults.GetExamResultsByUserIdPaging;
using Examination.Application.Queries.ExamResults.GetExamResultsPaging;
using Examination.Shared.ExamResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Examination.API.Controllers
{
    public class ExamResultsController : BaseController
    {
        private readonly IMediator _mediator;

        public ExamResultsController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExamResultByIdAsync(string id)
        {
            var result = await _mediator.Send(new GetExamResultByIdQuery(id));
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("finish")]
        public async Task<IActionResult> FinishExamAsync(FinishExamRequest request)
        {
            var result = await _mediator.Send(new FinishExamCommand() { ExamResultId = request.ExamResultId });
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("skip")]
        public async Task<IActionResult> SkipExamAsync(SkipExamRequest request)
        {
            var result = await _mediator.Send(new SkipExamCommand() { ExamResultId = request.ExamResultId });
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("next-question")]
        public async Task<IActionResult> NextQuestionAsync(NextQuestionRequest request)
        {
            var result = await _mediator.Send(new SubmitQuestionCommand() { QuestionId = request.QuestionId, ExamResultId = request.ExamResultId, AnswerIds = request.AnswerIds });
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartExamAsync(StartExamRequest request)
        {
            var result = await _mediator.Send(new StartExamCommand() { ExamId = request.ExamId });
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetExamResultsByUserIdPaging([FromQuery] GetExamResultsByUserIdPagingQuery query)
        {
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetExamResultsPaging([FromQuery] GetExamResultsPagingQuery query)
        {
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }
    }
}
