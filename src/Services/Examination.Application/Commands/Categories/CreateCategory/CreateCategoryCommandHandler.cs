using AutoMapper;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Shared.Categories;
using Examination.Shared.SeedWorks;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Examination.Application.Commands.Categories.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ApiResult<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;

        public CreateCategoryCommandHandler(
                ICategoryRepository categoryRepository,
                ILogger<CreateCategoryCommandHandler> logger,
                 IMapper mapper
            )
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _mapper = mapper;

        }

        public async Task<ApiResult<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var itemToAdd = await _categoryRepository.GetCategoriesByNameAsync(request.Name);
            if (itemToAdd != null)
            {
                _logger.LogError($"Category with name: {request.Name} already exists");
                return null;
            }
            itemToAdd = new Category(ObjectId.GenerateNewId().ToString(), request.Name, request.UrlPath);
            await _categoryRepository.InsertAsync(itemToAdd);
            var result = _mapper.Map<CategoryDto>(itemToAdd);
            return new ApiSuccessResult<CategoryDto>(200, result);
        }
    }
}
