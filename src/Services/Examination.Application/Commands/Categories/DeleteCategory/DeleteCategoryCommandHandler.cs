using AutoMapper;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Shared.SeedWorks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Examination.Application.Commands.Categories.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ApiResult<bool>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteCategoryCommandHandler> _logger;

        public DeleteCategoryCommandHandler(
                ICategoryRepository categoryRepository,
                ILogger<DeleteCategoryCommandHandler> logger,
                 IMapper mapper
            )
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _mapper = mapper;

        }
        public async Task<ApiResult<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var itemToDelete = await _categoryRepository.GetCategoriesByIdAsync(request.Id);
            if (itemToDelete == null)
            {
                _logger.LogError("Category not found");
                return new ApiErrorResult<bool>(400, "Category not found");
            }
            try
            {
                await _categoryRepository.DeleteAsync(request.Id);
                return new ApiSuccessResult<bool>(200, true, "Delete successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
