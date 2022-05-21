using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace UtilityEngine.Sql
{
    public class QueryManager
    {
        private string connectionString;
        public QueryManager()
        {
            connectionString = ConfigManager.GetConnectionString();
        }
        public Test GetTestFromDb(Guid guid)
        {
            Test test = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(Queries.Transactions.GetTestName);
                SqlCommand getTestCommand = new SqlCommand(Queries.Tests.GetTestCommandText, connection);
                getTestCommand.Transaction = transaction;

                test = GetTest(guid, getTestCommand);
                ClearParameters(getTestCommand);

                List<Question> questions = GetQuestions(test.Code, getTestCommand);
                test.Questions = questions;
                ClearParameters(getTestCommand);

                List<AnswerVariant> answers = GetAnswersVariants(getTestCommand, test.Code).ToList();
                ClearParameters(getTestCommand);

                foreach(Question question in questions)
                {
                    question.AnswerVariants = GetAnswersForQuestion(answers, question.Code);
                }

                getTestCommand.Transaction.Commit();
                connection.Close();
            }

            return test;
        }

        public bool DeleteTest(Guid guid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                connection.Open();
                SqlCommand command = new SqlCommand(Queries.Tests.DeleteTestProcedureText, connection);
                ClearParameters(command);
                command.Parameters.Add(new SqlParameter("@guid", guid));
                command.Transaction = connection.BeginTransaction(Queries.Transactions.DeleteTestName);
                

                try
                {
                    command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    string s = e.Message;
                    command.Transaction.Rollback();
                    return false;
                }

                connection.Close();
            }
                
            return true;
        }

        private void ClearParameters(SqlCommand command)
        {
            command.Parameters.Clear();
        }
        private IEnumerable<AnswerVariant> GetAnswersVariants(SqlCommand command, int testCode)
        {
            command.CommandText = Queries.Answers.GetAnswersCommandText;
            command.Parameters.Add(new SqlParameter(Queries.Answers.TestCodeParameter, testCode));

            SqlDataReader answersReader = command.ExecuteReader();
            if (answersReader.HasRows)
            {
                while (answersReader.Read())
                {
                    string answerText = answersReader.GetString(Queries.Answers.AnswerText);
                    bool answerFlag = answersReader.GetBoolean(Queries.Answers.AnswerFlag);
                    int questionCode = answersReader.GetInt32(Queries.Answers.QuestionCode);

                    yield return new AnswerVariant(answerText, answerFlag, questionCode);
                }
            }

            answersReader.Close();
        }

        private List<AnswerVariant> GetAnswersForQuestion(IEnumerable<AnswerVariant> answers, int questionCode)
        {
            return answers.Where(x => x.QuestionCode == questionCode).ToList();
        }
        private List<Question> GetQuestions(int testCode, SqlCommand command)
        {
            List<Question> questions = new List<Question>();
            command.CommandText = Queries.Questions.GetQuestionsCommandText;
            command.Parameters.Add(new SqlParameter(Queries.Questions.TestCodeParameter, testCode));
            SqlDataReader questionsReader = null;
            try
            {
                questionsReader = command.ExecuteReader();
            }
            catch
            {
                command.Transaction.Rollback();
            }
            

            if (questionsReader.HasRows)
            {
                while (questionsReader.Read())
                {
                    int questionCode = questionsReader.GetInt32(Queries.Questions.QuestionCode);
                    string questionText = questionsReader.GetString(Queries.Questions.QuestionText);
                    questions.Add(new Question(questionCode, questionText));
                }
            }

            questionsReader.Close();
            return questions;
        }
        private Test GetTest(Guid guid, SqlCommand command)
        {
            command.Parameters.Add(new SqlParameter(Queries.Tests.GuidParameter, guid));
            return ReadTestFromDb(command);
        }

        private static Test ReadTestFromDb(SqlCommand command)
        {
            Test test = null;
            SqlDataReader testReader = null;
            try
            {
                testReader = command.ExecuteReader();
            }
            catch
            {
                command.Transaction.Rollback();
                return null;
            }

            if (testReader.HasRows)
            {
                while (testReader.Read())
                {
                    int testCode = testReader.GetInt32(Queries.Tests.TestCode);
                    string testTitle = testReader.GetString(Queries.Tests.TestTitle);
                    Guid testGuid = testReader.GetGuid(Queries.Tests.TestGUID);
                    string testTheory = testReader.IsDBNull(Queries.Tests.Theory) ? null : testReader.GetString(Queries.Tests.Theory);
                    bool theoryFlag = testReader.GetBoolean(Queries.Tests.TheoryFlag);
                    string theorySourse = testReader.GetString(Queries.Tests.TheorySource);
                    TimeSpan testTime = testReader.GetTimeSpan(Queries.Tests.TestTime);
                    int questionAmount = testReader.GetInt32(Queries.Tests.QuestionsAmount);
                    int questionForPass = testReader.GetInt32(Queries.Tests.AmountForPass);
                    byte[] image = testReader.IsDBNull(Queries.Tests.Image) ? null : (byte[])testReader[Queries.Tests.Image];
                    test = new Test(testCode, testTitle, testGuid, testTheory, theoryFlag, theorySourse, testTime, questionAmount, questionForPass, image);
                }
            }

            testReader.Close();
            return test;
        }

        public bool WriteTestToDb(Test test, out string errorMessage)
        {
            errorMessage = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(Queries.Transactions.AddTestName);
                SqlCommand writeCommand = new SqlCommand(Queries.Tests.AddTestCommandText, connection);
                //writeCommand.Parameters.Add(new SqlParameter(Queries.Tests.GuidParameter, test.Guid));
                writeCommand.Transaction = transaction;

                //if (IsExists(writeCommand))
                //{
                //    errorMessage = "this test already exists in database";
                //    return false;
                //}

                if (!WriteTest(test, writeCommand, out int testCode))
                {
                    return false;
                }

                if (!WriteQuestions(test.Questions, testCode, writeCommand))
                {
                    return false;
                }

                writeCommand.Transaction.Commit();
                connection.Close();
            }

            return true;
        }

        private bool WriteQuestions(List<Question> questions, int testCode, SqlCommand command)
        {
            foreach (Question question in questions)
            {
                int questionCode = 0;
                command.CommandText = Queries.Questions.AddQuestionCommandText;
                command.Parameters.Add(new SqlParameter(Queries.Questions.TestCodeParameter, testCode));
                command.Parameters.Add(new SqlParameter(Queries.Questions.TextParameter, question.Text));
                try
                {
                    questionCode = (int)command.ExecuteScalar();
                }
                catch
                {
                    command.Transaction.Rollback();
                    return false;
                }

                command.Parameters.Clear();
                if(!WriteAnswers(command, question.AnswerVariants, questionCode))
                {
                    return false;
                }
            }

            return true;
        }

        private bool WriteAnswers(SqlCommand command, List<AnswerVariant> answers, int questionCode)
        {
            
            foreach (AnswerVariant answerVariant in answers)
            {
                if(!WriteAnswer(command, answerVariant, questionCode))
                {
                    return false;
                }
            }

            return true;
        }

        private bool WriteAnswer(SqlCommand command, AnswerVariant answerVariant, int questionCode)
        {
            command.CommandText = Queries.Answers.AddAnswersProcedureText;
            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter(Queries.Answers.CodeParameter, questionCode));
            command.Parameters.Add(new SqlParameter(Queries.Answers.TextParameter, answerVariant.Text));
            command.Parameters.Add(new SqlParameter(Queries.Answers.FlagParameter, answerVariant.IsTrue));

            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                command.Transaction.Rollback();
                return false;
            }

            command.Parameters.Clear();
            return true;
        }

        private bool WriteTest(Test test, SqlCommand command, out int testCode)
        {
            testCode = 0;

            if (!WriteTheory(test.Theory, command, out int theoryCode))
            {
                command.Transaction.Rollback();
                return false;
            }

            if (!WriteImage(test.Image, command, out int imageCode))
            {
                command.Transaction.Rollback();
                return false;
            }

            command.CommandText = Queries.Tests.AddTestCommandText;
            object theory = theoryCode == 0 ? DBNull.Value : (object)theoryCode;
            object image = theoryCode == 0 ? DBNull.Value : (object)imageCode;
            testCode = 0;
            command.Parameters.Add(new SqlParameter(Queries.Tests.TitleParameter, test.Title));
            command.Parameters.Add(new SqlParameter(Queries.Tests.GuidParameter, test.Guid));
            command.Parameters.Add(new SqlParameter(Queries.Tests.TheoryFlagParameter, test.TheoryFlag));
            command.Parameters.Add(new SqlParameter(Queries.Tests.TheorySourseParameter, test.Url));
            command.Parameters.Add(new SqlParameter(Queries.Tests.TimeParameter, test.Time));
            command.Parameters.Add(new SqlParameter(Queries.Tests.QuestionsAmountParameter, test.QuestionsAmount));
            command.Parameters.Add(new SqlParameter(Queries.Tests.AmountForPassParameter, test.QuestionsForPass));
            command.Parameters.Add(new SqlParameter(Queries.Tests.TheoryParameter, theory));
            command.Parameters.Add(new SqlParameter(Queries.Tests.ImageParameter, image));

            try
            {
                testCode = (int)command.ExecuteScalar();
            }
            catch
            {
                command.Transaction.Rollback();
                return false;
            }

            ClearParameters(command);
            return true;
        }

        private bool IsExists(SqlCommand command)
        {
            object result = command.ExecuteScalar();
            ClearParameters(command);
            return Convert.ToBoolean(result);
        }

        private bool WriteTheory(string data, SqlCommand command, out int theoryCode)
        {
            theoryCode = 0;
            if (string.IsNullOrEmpty(data))
            {
                return true;
            }

            command.CommandText = Queries.Theory.AddTheory;
            command.Parameters.Add(new SqlParameter(Queries.Theory.DataParameter, data));
            try
            {
                theoryCode = (int)command.ExecuteScalar();
            }
            catch
            {
                command.Transaction.Rollback();
                return false;
            }

            ClearParameters(command);
            return true;
        }
        private bool WriteImage(byte[] data, SqlCommand command, out int imageCode)
        {
            imageCode = 0;
            if(data == null)
            {
                return true;
            }

            command.CommandText = Queries.Images.AddImage;
            command.Parameters.Add(new SqlParameter(Queries.Theory.DataParameter, data));
            try
            {
                imageCode = (int)command.ExecuteScalar();
            }
            catch
            {
                command.Transaction.Rollback();
                return false;
            }

            ClearParameters(command);
            return true;
        }
    }
}
