using Examination.Shared.Exams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortalApp.Services.Interfaces;

namespace PortalApp.Pages.Exams
{
    public class ExamResultModel : PageModel
    {
        private readonly IExamService _examService;

        [BindProperty]
        public ExamDto Exam { get; set; }

        public ExamResultModel(IExamService examService)
        {
            _examService = examService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var result = await _examService.GetExamByIdAsync(id);
            if (!result.IsSuccessed)
            {
                return NotFound();
            }
            Exam = result.ResultObj;
            return Page();
        }
    }
}
