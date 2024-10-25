using AutoMapper;
using Examination.Application.Queries.Categories.GetCategoriesPaging;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.Categories;
using Examination.Shared.Questions;
using Examination.Shared.SeedWorks;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Queries.Questions.GetQuestionsPaging
{
    public class GetQuestionsPagingQueryHandler : IRequestHandler<GetQuestionsPagingQuery, ApiResult<PagedList<QuestionDto>>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IClientSessionHandle _clientSessionHandle;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCategoriesPagingQueryHandler> _logger;
        public GetQuestionsPagingQueryHandler(IQuestionRepository questionRepository,
                IMapper mapper,
                ILogger<GetCategoriesPagingQueryHandler> logger,
                IClientSessionHandle clientSessionHandle)
        {
            _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
            _clientSessionHandle = clientSessionHandle ?? throw new ArgumentNullException(nameof(_clientSessionHandle));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResult<PagedList<QuestionDto>>> Handle(GetQuestionsPagingQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("BEGIN: GetHomeExamListQueryHandler");

            var result = await _questionRepository.GetQuestionsPagingAsync(request.CategoryId, request.SearchKeyword, request.PageIndex, request.PageSize);
            var items = _mapper.Map<List<QuestionDto>>(result.Items);

            _logger.LogInformation("END: GetHomeExamListQueryHandler");
            var pagedResult = new PagedList<QuestionDto>(items, result.MetaData.TotalCount, request.PageIndex, request.PageSize);
            return new ApiSuccessResult<PagedList<QuestionDto>>(200, pagedResult);
        }
    }
}
