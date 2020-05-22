using Newtonsoft.Json.Linq;
using OrdonnancementsEquitables.Jobs;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace OrdonnancementsEquitables.Parsers
{
    public class Parser
    {
        private static Type GetTypeFromJSON(JObject obj)
        {
            string type = obj["job_type"].Value<string>();
            Type jobType = Type.GetType(typeof(Job).Namespace + "." + type);
            return jobType;
        }

        private static JObject ParseToJSON(string content) => JObject.Parse(content);

        private static void ParseAndGetType(string filePath, out JObject obj, out Type jobType)
        {
            string content = File.ReadAllText(filePath);
            obj = ParseToJSON(content);
            jobType = GetTypeFromJSON(obj);
        }

        private static Job[] ParseJobsFromJSON(JObject obj, Type jobType)
        {
            JArray list = (JArray)obj["job_list"];
            var arr = list.Select(t => (Job)t.ToObject(jobType)).ToArray();
            return arr;
        }

        private static TJob[] ParseJobsFromJSON<TJob>(JObject obj, Type jobType) where TJob : Job
        {
            if (jobType != typeof(TJob))
            {
                throw new ParserTypeException();
            }

            JArray list = obj["job_list"].Value<JArray>();

            var arr = list.Select(t => t.ToObject<TJob>()).ToArray();
            return arr;
        }

        public static Job[] ParseFromFile(string filePath, out Type jobType)
        {
            ParseAndGetType(filePath, out JObject obj, out jobType);
            return ParseJobsFromJSON(obj, jobType);
        }

        public static TJob[] ParseFromFile<TJob>(string filePath) where TJob : Job
        {
            ParseAndGetType(filePath, out JObject obj, out Type jobType);
            return ParseJobsFromJSON<TJob>(obj, jobType);
        }
    }
}
