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
        public static Job[] ParseFromContent(string content, out Type outJobType)
        {
            JObject obj = JObject.Parse(content);

            string type = obj["job_type"].Value<string>();
            Type jobType = Type.GetType("OrdonnancementsEquitables.Jobs." + type);
            outJobType = jobType;
            JArray list = (JArray)obj["job_list"];
            var arr = list.Select(t => (Job)t.ToObject(jobType)).ToArray();
            return arr;
        }

        public static TJob[] ParseFromContent<TJob>(string content) where TJob : Job
        {
            JObject obj = JObject.Parse(content);

            string type = obj["job_type"].Value<string>();
            Type jobType = Type.GetType("OrdonnancementsEquitables.Jobs." + type);

            if (jobType != typeof(TJob))
            {
                throw new ParserTypeException();
            }

            JArray list = obj["job_list"].Value<JArray>();

            var arr = list.Select(t => t.ToObject<TJob>()).ToArray();
            return arr;
        }

        public static Job[] ParseFromFile(string filePath, out Type outJobType)
        {
            string content = File.ReadAllText(filePath);
            return ParseFromContent(content, out outJobType);
        }

        public static TJob[] ParseFromFile<TJob>(string filePath) where TJob : Job
        {
            string content = File.ReadAllText(filePath);
            return ParseFromContent<TJob>(content);
        }
    }
}
