using Examination.Shared.ExamResults;
using Examination.Shared.Exams;
using Examination.Shared.SeedWorks;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Queries.ExamResults.GetExamResultsPaging
{
    public class GetExamResultsPagingQuery : IRequest<ApiResult<PagedList<ExamResultDto>>>
    {
        public string Keyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}