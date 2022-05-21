using System;
using System.Collections.Generic;
using System.Linq;
using UtilityEngine.Serialization;
using UtilityEngine.Sql;
using UtilityEngineORM;

namespace ImportExportUtility
{
    class Program
    {
        static QueryManager queryManager = new QueryManager();
        static void Main(string[] args)
        {
            //GetTestFromDbORM(new Guid("56FFC0BF-13A4-4821-9DF3-9418A4740B01"));
            GetTestFromDbORM(new Guid("56FFC0BF-13A4-4821-9DF3-9418A4740B01"));
            //LoadTestToDbORM("Вторая_Мировая_война1");
            //LoadTestToDb("Вторая_Мировая_война1");
            //DeleteTest(new Guid("56FFC0BF-13A4-4821-9DF3-9418A4740B02"));
            Console.ReadKey();
        }

        static void DeleteTest(Guid guid)
        {
            if (queryManager.DeleteTest(guid))
            {
                Console.WriteLine("Successful deleted");
            }
            else
            {
                Console.WriteLine("Not Successful");
            }
        }
        static void GetTestFromDb(Guid guid)
        {
            Entities.Test test = queryManager.GetTestFromDb(guid);
            if(test == null)
            {
                Console.WriteLine("Not Successful");
                return;
            }
            SerializationManager.Serialize(test);
            Console.WriteLine("Successful");
        }
        static void LoadTestToDb(string fileName)
        {
            Entities.Test test = SerializationManager.Deserialize(fileName);
            
            if (!queryManager.WriteTestToDb(test, out string errorMessage))
            {
                Console.WriteLine(errorMessage);
            }
            else
            {
                Console.WriteLine("Successful");
            }
        }
        static void GetTestFromDbORM(Guid guid)
        {
            Entities.Test test = GetTest(guid);
            if (test == null)
            {
                Console.WriteLine("Not Successful");
                return;
            }

            SerializationManager.Serialize(test);
            Console.WriteLine("Successful");
        }
        static void LoadTestToDbORM(string fileName)
        {
            QueryManagerORM queryManager = new QueryManagerORM();
            Entities.Test test = SerializationManager.Deserialize(fileName);
            if (queryManager.SaveTestToDbORM(test))
            {
                Console.WriteLine("Successful");
            }
            else
            {
                Console.WriteLine("Fail");
            }
        }

        static Entities.Test GetTest(Guid guid)
        {
            QueryManagerORM queryManager = new QueryManagerORM();
            Test testData = queryManager.GetTestData(guid);
            Entities.Test test = Converter.ConvertDataToTest(testData);

            return test;
        }
    }
}
