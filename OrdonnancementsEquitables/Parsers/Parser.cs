using Newtonsoft.Json.Linq;
using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Models;
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
        public bool? IsSingleUser { get; }

        private Parser() { }

        public Parser(string filePath) 
        {
            string content = File.ReadAllText(filePath);
            JObject obj = ParseToJSON(content);
            JobType = GetTypeFromJSON(obj);
            list = GetJobsArrayFromJSON(obj);

            var childType = list.First.Type;
            IsSingleUser = childType switch
            {
                JTokenType.Object => true,
                JTokenType.Array => false,
                _ => null
            };
        }

        JObject ParseToJSON(string content) => JObject.Parse(content);
        Type GetTypeFromJSON(JObject obj)
        {
            string type = obj["job_type"].Value<string>();
            Type jobType = Type.GetType(typeof(Job).Namespace + "." + type);
            return jobType;
        }

        JArray GetJobsArrayFromJSON(JObject obj) => (JArray)obj["job_list"];

        TJob[] GetJobArray<TJob>(JArray token) => token.Select(t => t.ToObject<TJob>()).ToArray();

        public TJob[] ParseJobsFromJSON<TJob>() where TJob : Job
        {
            if (IsSingleUser == false)
                return null;
            if (JobType != typeof(TJob))
            {
                throw new ParserTypeException();
            }
            var arr = GetJobArray<TJob>(list);
            return arr;
        }

        public User<TJob>[] ParseUsersFromJSON<TJob>() where TJob : Job
        {
            if (IsSingleUser == true)
                return null;

            var users = list.Select(u => new User<TJob>(GetJobArray<TJob>((JArray)u))).ToArray();
            
            return users;
        }
    }
}
