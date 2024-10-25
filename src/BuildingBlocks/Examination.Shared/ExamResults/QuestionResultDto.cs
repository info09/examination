using Examination.Shared.Enums;

namespace Examination.Shared.ExamResults
{
    public class QuestionResultDto
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public QuestionType QuestionType { get; set; }
        public Level Level { get; set; }
        public string Explain { get; set; }
        public List<AnswerResultDto> Answers { set; get; }
        public bool? Result { get; set; }
    }
}
