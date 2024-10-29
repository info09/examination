using Examination.Application.Commands.Categories.CreateCategory;
using Examination.Application.Commands.Categories.DeleteCategory;
using Examination.Application.Commands.Categories.UpdateCategory;
using Examination.Application.Queries.Categories.GetAllCategories;
using Examination.Application.Queries.Categories.GetCategoriesPaging;
using Examination.Application.Queries.Categories.GetCategoryById;
using Examination.Shared.Categories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Examination.API.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(IMediator mediator, ILogger<CategoriesController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryByIdAsync(string id)
        {
            var query = new GetCategoryByIdQuery(id);
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var query = new GetAllCategoriesQuery();
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetCategoriesPagingAsync([FromQuery] GetCategoriesPagingQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoriesAsync([FromBody] CreateCategoryRequest request)
        {
            var command = new CreateCategoryCommand()
            {
                Name = request.Name,
                UrlPath = request.UrlPath,
            };
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteCategoryAsync(string id)
        {
            _logger.LogInformation("BEGIN: DeleteCategoryAsync");

            var result = await _mediator.Send(new DeleteCategoryCommand(id));

            _logger.LogInformation("END: DeleteCategoryAsync");
            return StatusCode(result.StatusCode, result);
        }


        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] UpdateCategoryRequest request)
        {
            _logger.LogInformation("BEGIN: UpdateCategoryAsync");

            var result = await _mediator.Send(new UpdateCategoryCommand()
            {
                Id = request.Id,
                Name = request.Name,
                UrlPath = request.UrlPath
            });

            _logger.LogInformation("END: UpdateCategoryAsync");
            return StatusCode(result.StatusCode, result);
        }   
    }
}
