namespace Examination.Shared.ExamResults
{
    public class NextQuestionRequest
    {
        public string ExamResultId { get; set; }

        public string QuestionId { get; set; }

        public List<string> AnswerIds { get; set; }
    }
}
