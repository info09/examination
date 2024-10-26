using Examination.Application.Commands.Categories.CreateCategory;
using Examination.Application.Commands.Categories.DeleteCategory;
using Examination.Application.Commands.Categories.UpdateCategory;
using Examination.Application.Commands.Questions.CreateQuestion;
using Examination.Application.Commands.Questions.DeleteQuestion;
using Examination.Application.Commands.Questions.UpdateQuestion;
using Examination.Application.Queries.Categories.GetAllCategories;
using Examination.Application.Queries.Categories.GetCategoriesPaging;
using Examination.Application.Queries.Categories.GetCategoryById;
using Examination.Application.Queries.Questions.GetQuestionById;
using Examination.Application.Queries.Questions.GetQuestionsPaging;
using Examination.Shared.Categories;
using Examination.Shared.Questions;
using MediatR;
using Microsoft.AspNetCore.Http;
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
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetQuestionsPagingAsync([FromQuery] GetQuestionsPagingQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
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
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteQuestionAsync(string id)
        {
            _logger.LogInformation("BEGIN: DeleteQuestionAsync");

            var result = await _mediator.Send(new DeleteQuestionCommand(id));

            _logger.LogInformation("END: DeleteQuestionAsync");
            return Ok(result);
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
            return Ok(result);
        }
    }
}
