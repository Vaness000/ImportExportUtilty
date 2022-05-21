using System;

namespace Entities
{
    [Serializable]
    public class AnswerVariant
    {
        public int QuestionCode;
        public string Text { get; set; }
        public bool IsTrue { get; set; }

        public AnswerVariant(string text, bool isTrue, int questionCode)
        {
            Text = text;
            IsTrue = isTrue;
            QuestionCode = questionCode;
        }

        public AnswerVariant()
        {

        }
    }
}
