using AutoMapper;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Shared.Categories;
using Examination.Shared.SeedWorks;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Examination.Application.Queries.Categories.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, ApiResult<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IClientSessionHandle _clientSessionHandle;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCategoryByIdQueryHandler> _logger;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, IClientSessionHandle clientSessionHandle, IMapper mapper, ILogger<GetCategoryByIdQueryHandler> logger)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _clientSessionHandle = clientSessionHandle ?? throw new ArgumentNullException(nameof(clientSessionHandle));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResult<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("BEGIN: GetCategoryByIdQueryHandler");

            var result = await _categoryRepository.GetCategoriesByIdAsync(request.Id);
            var item = _mapper.Map<CategoryDto>(result);

            _logger.LogInformation("END: GetCategoryByIdQueryHandler");

            return new ApiSuccessResult<CategoryDto>(200, item);
        }
    }
}
