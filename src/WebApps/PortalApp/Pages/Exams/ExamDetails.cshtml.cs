using Examination.Shared.Exams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortalApp.Services.Interfaces;

namespace PortalApp.Pages.Exams
{
    public class ExamDetailModel : PageModel
    {
        private readonly IExamService _examService;
        private readonly IExamResultService _examResultService;

        [BindProperty]
        public ExamDto Exam { get; set; }

        public ExamDetailModel(IExamService examService, IExamResultService examResultService)
        {
            _examService = examService;
            _examResultService = examResultService;
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

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _examResultService.StartExamAsync(new Examination.Shared.ExamResults.StartExamRequest() { ExamId = Exam.Id });

            if (!result.IsSuccessed)
            {
                return NotFound();
            }
            return Redirect($"/take-exam.html?examResultId={result.ResultObj.Id}");
        }
    }
}
