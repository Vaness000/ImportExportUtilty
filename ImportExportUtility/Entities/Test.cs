using System;
using System.Collections.Generic;

namespace Entities
{
    [Serializable]
    public class Test
    {
        [NonSerialized]
        public int Code;
        public string Title { get; set; }
        public Guid Guid { get; set; }
        public string Theory { get; set; }
        public bool TheoryFlag { get; set; }
        public string Url { get; set; }
        public TimeSpan Time { get; set; }
        public int QuestionsAmount { get; set; }
        public int QuestionsForPass { get; set; }
        public byte[] Image { get; set; }
        public List<Question> Questions { get; set; }
        public Test(int code, string title, Guid guid, string theory, bool theoryFlag, string url, 
            TimeSpan time, int questionsAmount, int questionsForPass, byte[] image, List<Question> questions = null)
        {
            Code = code;
            Title = title;
            Guid = guid;
            Theory = theory;
            TheoryFlag = theoryFlag;
            Url = url;
            Time = time;
            QuestionsAmount = questionsAmount;
            QuestionsForPass = questionsForPass;
            Questions = questions;
            Image = image;
        }

        public Test()
        {

        }
        public override string ToString()
        {
            return Title;
        }
    }
}
