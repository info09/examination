namespace Examination.Shared.ExamResults
{
    public class AnswerResultDto
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public bool? UserChosen { get; set; }

        public bool IsCorrect { set; get; }
    }
}
