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
        readonly JArray list;

        public Type JobType { get; }

        private Parser() { }

        public Parser(string filePath) 
        {
            string content = File.ReadAllText(filePath);
            JObject obj = ParseToJSON(content);
            JobType = GetTypeFromJSON(obj);
            list = GetJobsArrayFromJSON(obj);
        }

        JObject ParseToJSON(string content) => JObject.Parse(content);
        Type GetTypeFromJSON(JObject obj)
        {
            string type = obj["job_type"].Value<string>();
            Type jobType = Type.GetType(typeof(Job).Namespace + "." + type);
            return jobType;
        }
        JArray GetJobsArrayFromJSON(JObject obj) => (JArray)obj["job_list"];

        public TJob[] ParseJobsFromJSON<TJob>() where TJob : Job
        {
            if (JobType != typeof(TJob))
            {
                throw new ParserTypeException();
            }
            var arr = list.Select(t => t.ToObject<TJob>()).ToArray();
            return arr;
        }
    }
}
