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
        public static Job[] ParseFromContent(string content)
        {
            JObject obj = JObject.Parse(content);

            string type = obj["job_type"].Value<string>();
            Type jobType = Type.GetType("OrdonnancementsEquitables.Jobs." + type);

            JArray list = (JArray)obj["job_list"];
            var arr = list.Select(t => (Job)t.ToObject(jobType)).ToArray();
            return arr;
        }

        public static Job[] ParseFromFile(string filePath)
        {
            string content = File.ReadAllText(filePath);
            return ParseFromContent(content);
        }
    }

    public class Parser<TJob> where TJob : Job
    {
        public static TJob[] ParseFromContent(string content)
        {
            JObject obj = JObject.Parse(content);

            string type = obj["job_type"].Value<string>();
            Type jobType = Type.GetType("OrdonnancementsEquitables.Jobs." + type);

            if (jobType != typeof(TJob))
            {
                throw new ParserTypeException("Parser's and JSON's Job Types don't match");
            }

            JArray list = (JArray)obj["job_list"];

            var arr = list.Select(t => (TJob)t.ToObject(jobType)).ToArray();
            return arr;
        }

        public static TJob[] ParseFromFile(string filePath)
        {
            string content = File.ReadAllText(filePath);
            return ParseFromContent(content);
        }
    }
}
