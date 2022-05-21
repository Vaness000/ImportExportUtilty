using System.Collections.Generic;
using System.Linq;

namespace UtilityEngineORM
{
    public class Converter
    {
        public static Entities.Test ConvertDataToTest(Test testData)
        {
            if (testData == null)
            {
                return null;
            }

            string theory = ConvertDataToTheory(testData);
            byte[] image = ConvertDataToImage(testData);

            Entities.Test test = new Entities.Test(testData.Test_code, testData.Test_title, testData.Test_GUID, theory, testData.Theory_flag,
                testData.Theory_Source, testData.Test_time, testData.Questions_amount, testData.Amount_for_pass, image);
            test.Questions = ConvertDataToQuestions(testData.Questions.ToList()).ToList();

            return test;
        }

        private static IEnumerable<Entities.Question> ConvertDataToQuestions(List<Question> questions)
        {
            foreach (Question questionData in questions)
            {
                List<Entities.AnswerVariant> answersVariants = ConvertDataToAnswers(questionData).ToList();
                yield return new Entities.Question(questionData.Question_code, questionData.Question_text, answersVariants);
            }
        }

        private static IEnumerable<Entities.AnswerVariant> ConvertDataToAnswers(Question question)
        {
            foreach (Answer answer in question.Answers)
            {
                yield return new Entities.AnswerVariant(answer.Answer_text, answer.Answer_flag, answer.Question_code);
            }
        }

        private static byte[] ConvertDataToImage(Test testData)
        {
            byte[] image = null;
            if (testData.Image_code.HasValue)
            {
                image = testData.Image.Image_data;
            }

            return image;
        }

        private static string ConvertDataToTheory(Test testData)
        {
            string theory = null;
            if (testData.Theory_code.HasValue)
            {
                theory = testData.Theory.Theory_data;
            }

            return theory;
        }
    }
}
