using Entities;
using System.IO;
using System.Xml.Serialization;

namespace UtilityEngine.Serialization
{
    public class SerializationManager
    {
        public static void Serialize(Test test)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Test));
            string path = string.Format("{0}.xml", test.Title.Replace(' ', '_'));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, test);
            }
        }

        public static Test Deserialize(string testTitle)
        {
            Test test = null;
            string path = string.Format("{0}.xml", testTitle);
            XmlSerializer serializer = new XmlSerializer(typeof(Test));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                test = (Test)serializer.Deserialize(fs);
            }

            return test;
        }
    }
}
