using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityEngineORM
{
    public class Logger
    {
        private const string path = "log.txt";
        private string readTestOperation = string.Format("Reading test from database{0}", Environment.NewLine);
        private string loadTestOperation = string.Format("Loading test from database{0}", Environment.NewLine);
        public Logger(bool isLoadingTest)
        {
            File.WriteAllText(path, isLoadingTest ? loadTestOperation : readTestOperation);
        }

        public void Log(string content)
        {
            File.AppendAllText(path, content);
        }
    }
}
