using System;
using System.Collections.Generic;

namespace Entities
{
    [Serializable]
    public class Question
    {
        [NonSerialized]
        public int Code;
        public string Text { get; set; }
        public List<AnswerVariant> AnswerVariants { get; set; }

        public Question(int code, string text, List<AnswerVariant> answerVariants = null)
        {
            Code = code;
            Text = text;
            AnswerVariants = answerVariants;
        }
        public Question()
        {

        }
    }
}
