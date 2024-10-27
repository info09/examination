using Examination.Application.Commands.Questions.CreateQuestion;
using Examination.Application.Commands.Questions.DeleteQuestion;
using Examination.Application.Commands.Questions.UpdateQuestion;
using Examination.Application.Queries.Questions.GetQuestionById;
using Examination.Application.Queries.Questions.GetQuestionsPaging;
using Examination.Shared.Questions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Examination.API.Controllers
{
    public class QuestionsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<QuestionsController> _logger;

        public QuestionsController(IMediator mediator, ILogger<QuestionsController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionByIdAsync(string id)
        {
            var query = new GetQuestionByIdQuery(id);
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetQuestionsPagingAsync([FromQuery] GetQuestionsPagingQuery query)
        {
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestionsAsync([FromBody] CreateQuestionRequest request)
        {
            var command = new CreateQuestionCommand()
            {
                Content = request.Content,
                QuestionType = request.QuestionType,
                Level = request.Level,
                CategoryId = request.CategoryId,
                Answers = request.Answers,
                Explain = request.Explain,
            };
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteQuestionAsync(string id)
        {
            _logger.LogInformation("BEGIN: DeleteQuestionAsync");

            var result = await _mediator.Send(new DeleteQuestionCommand(id));

            _logger.LogInformation("END: DeleteQuestionAsync");
            return StatusCode(result.StatusCode, result);
        }


        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateQuestionAsync([FromBody] UpdateQuestionRequest request)
        {
            _logger.LogInformation("BEGIN: UpdateQuestionAsync");

            var result = await _mediator.Send(new UpdateQuestionCommand()
            {
                Id = request.Id,
                Content = request.Content,
                QuestionType = request.QuestionType,
                Level = request.Level,
                CategoryId = request.CategoryId,
                Answers = request.Answers,
                Explain = request.Explain
            });

            _logger.LogInformation("END: UpdateQuestionAsync");
            return StatusCode(result.StatusCode, result);
        }
    }
}
