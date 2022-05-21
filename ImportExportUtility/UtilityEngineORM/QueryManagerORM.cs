using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using System.Data.Entity;

namespace UtilityEngineORM
{
    public class QueryManagerORM
    {
        //считать тест из бд
        //новая реализация
        public Test GetTestData(Guid guid)
        {
            Logger logger = new Logger(false);
            using (QuizEntities db = new QuizEntities())
            {
                db.Database.Log = logger.Log;
                using (var transaction = db.Database.BeginTransaction())//TODO include entity framework
                {
                    try
                    {
                        return db.Tests.Include(x => x.Questions)
                                       .Include(x => x.Image)
                                       .Include(x => x.Theory)
                                       .Include(x => x.Questions.Select(a => a.Answers))
                                       .FirstOrDefault(x => x.Test_GUID == guid);//TODO уточнить когда происходит выполнение запроса
                    }
                    catch
                    {
                        db.Database.CurrentTransaction.Rollback();
                        return null;
                    }
                }
            }
        }

        //записать тест в бд
        public bool SaveQuestions(List<Entities.Question> questions, Test testData, QuizEntities db)
        {
            foreach (Entities.Question question in questions)
            {
                Question questionData = new Question
                {
                    Test = testData,
                    Question_text = question.Text
                };

                try
                {
                    db.Questions.Add(questionData);
                    db.SaveChanges();
                }
                catch
                {
                    db.Database.CurrentTransaction.Rollback();
                    return false;
                }

                if (!SaveAnswers(question.AnswerVariants, questionData, db))
                {
                    return false;
                }
            }

            return true;
        }

        private bool SaveImage(byte[] imageData, QuizEntities db, out Image image)
        {
            image = null;

            if (imageData != null)
            {
                image = new Image
                {
                    Image_data = imageData
                };

                try
                {
                    db.Images.Add(image);
                    db.SaveChanges();
                }
                catch
                {
                    db.Database.CurrentTransaction.Rollback();
                    return false;
                }
            }

            return true;
        }

        private bool SaveTheory(string theoryData, QuizEntities db, out Theory theory)
        {
            theory = null;

            if (theoryData != null)
            {
                theory = new Theory
                {
                    Theory_data = theoryData
                };

                try
                {
                    db.Theories.Add(theory);
                    db.SaveChanges();
                }
                catch
                {
                    db.Database.CurrentTransaction.Rollback();
                    return false;
                }
            }

            return true;
        }

        private bool SaveTest(Entities.Test test, Theory theory, Image image, QuizEntities db, out Test testData)
        {
            testData = new Test()
            {
                Test_GUID = test.Guid,
                Test_title = test.Title,
                Test_time = test.Time,
                Theory_flag = test.TheoryFlag,
                Theory_Source = test.Url,
                Questions_amount = test.QuestionsAmount,
                Amount_for_pass = test.QuestionsForPass,
                Theory = theory,
                Image = image
            };

            try
            {
                db.Tests.Add(testData);
                db.SaveChanges();
            }
            catch
            {
                testData = null;
                db.Database.CurrentTransaction.Rollback();
                return false;
            }

            return true;
        }

        private bool SaveAnswers(List<AnswerVariant> answers, Question questionData, QuizEntities db)
        {
            foreach (AnswerVariant answer in answers)
            {
                try
                {
                    db.Answers.Add(new Answer
                    {
                        Question = questionData,
                        Answer_text = answer.Text,
                        Answer_flag = answer.IsTrue
                    });
                    db.SaveChanges();
                }
                catch
                {
                    db.Database.CurrentTransaction.Rollback();
                    return false;
                }
            }

            return true;
        }

        public bool SaveTestToDbORM(Entities.Test test)
        {
            Logger logger = new Logger(true);
            using (QuizEntities db = new QuizEntities())
            {
                db.Database.Log = logger.Log;
                using (var transaction = db.Database.BeginTransaction())
                {
                    if(!SaveImage(test.Image, db, out Image image))
                    {
                        return false;
                    }

                    if(!SaveTheory(test.Theory, db, out Theory theory))
                    {
                        return false;
                    }

                    if (!SaveTest(test, theory, image, db, out Test testData))
                    {
                        return false;
                    }

                    if(!SaveQuestions(test.Questions, testData, db))
                    {
                        return false;
                    }

                    transaction.Commit();
                }
            }

            return true;
        }
    }
}
