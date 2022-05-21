using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityEngine.Sql
{
    public class Queries
    {
        public class Tests
        {
            //fields
            public const int TestCode = 0;
            public const int TestTitle = 1;
            public const int TestGUID = 2;
            public const int Theory = 3;
            public const int TheoryFlag = 4;
            public const int TheorySource = 5;
            public const int TestTime = 6;
            public const int QuestionsAmount = 7;
            public const int AmountForPass = 8;
            public const int Image = 9;

            //procedures
            public const string ExistsTestProcedureText = "ExistsTest";
            public const string DeleteTestProcedureText = "DeleteTest";
            public const string GetTestCommandText = "SELECT Test_code, Test_title, Test_GUID, Theory_data, Theory_flag, " +
                                                     "Theory_Source, Test_time, Questions_amount, Amount_for_pass, Image_data " +
                                                     "FROM Tests FULL JOIN Images on Tests.Image_Code = Images.ID " +
                                                     "FULL JOIN Theory on Theory.ID = Tests.Theory_code " +
                                                     "WHERE Test_GUID = @guid";
            //public const string GetTestIdCommandText = "SELECT Test_code FROM Tests WHERE Test_GUID = @guid";
            public const string AddTestCommandText = "INSERT INTO Tests(Test_title, Test_GUID, Theory_flag, Theory_Source, Test_time, Questions_amount, Amount_for_pass, Image_code, Theory_code) " +
                                                     "OUTPUT INSERTED.Test_code " +
                                                     "VALUES(@Title, @guid, @theory_flag, @theory_source, @test_time, @question_amount, @amount_for_pass, @image, @theory)";

            //parameters
            public const string GuidParameter = "@guid";
            public const string CodeParameter = "@code";
            public const string TitleParameter = "@Title";
            public const string TheoryParameter = "@theory";
            public const string TheoryFlagParameter = "@theory_flag";
            public const string TheorySourseParameter = "@theory_source";
            public const string TimeParameter = "@test_time";
            public const string QuestionsAmountParameter = "@question_amount";
            public const string AmountForPassParameter = "@amount_for_pass";
            public const string ImageParameter = "@image";
        }
        public class Questions
        {
            //fields
            public const int QuestionCode = 0;
            public const int QuestionText = 1;

            //procedures
            public const string GetQuestionsCommandText = "SELECT Question_code, Question_text FROM Questions where Test_code = @test_code";
            public const string AddQuestionCommandText = "INSERT INTO Questions OUTPUT INSERTED.Question_code VALUES (@test_code, @text)";
            //public const string GetLastQuestionProcedureText = "GetLastQuestion";

            //parameters
            public const string TestCodeParameter = "@test_code";
            public const string TextParameter = "@text";
        }
        public class Answers
        {
            //fields
            public const int AnswerText = 0;
            public const int AnswerFlag = 1;
            public const int QuestionCode = 2;

            //procedures
            public const string GetAnswersCommandText = "SELECT Answer_text, Answer_flag, Answers.Question_code " +
                                                        "FROM Answers inner join Questions on Questions.Question_code = Answers.Question_code " +
                                                        "WHERE Questions.Test_code = @test"; 
            public const string AddAnswersProcedureText = "INSERT INTO Answers VALUES (@question, @text, @flag)";

            //parameters
            public const string TestCodeParameter = "@test";
            public const string CodeParameter = "@question";
            public const string TextParameter = "@text";
            public const string FlagParameter = "@flag";
        }
        public class Transactions
        {
            public const string GetTestName = "getTestTransaction";
            public const string AddTestName = "addTestTransaction";
            public const string DeleteTestName = "addTestTransaction";
        }

        public class Theory
        {
            public const string AddTheory = "INSERT INTO Theory OUTPUT INSERTED.ID VALUES (@data)";

            public const string DataParameter = "@data";
        }

        public class Images
        {
            public const string AddImage = "INSERT INTO Images OUTPUT INSERTED.ID VALUES (@data)";

            public const string DataParameter = "@data";
        }
    }
}
