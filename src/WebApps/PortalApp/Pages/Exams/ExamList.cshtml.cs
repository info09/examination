using Examination.Shared.Exams;
using Examination.Shared.SeedWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortalApp.Services.Interfaces;

namespace PortalApp.Pages.Exams
{
    [Authorize]
    public class ExamListModel : PageModel
    {
        private readonly IExamService _examService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [BindProperty]
        public PagedList<ExamDto> Exam { get; set; }

        public ExamListModel(IExamService examService, IHttpContextAccessor httpContextAccessor)
        {
            _examService = examService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> OnGetAsync([FromQuery] ExamSearch examSearch)
        {
            var result = await _examService.GetExamsPagingAsync(examSearch);
            if (result.IsSuccessed)
            {
                Exam = result.ResultObj;
            }
            return Page();
        }
    }
}
