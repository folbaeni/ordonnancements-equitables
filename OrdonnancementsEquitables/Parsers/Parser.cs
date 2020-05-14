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

        public static TJob[] ParseFromContent<TJob>(string content) where TJob : Job
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

        public static Job[] ParseFromFile(string filePath)
        {
            string content = File.ReadAllText(filePath);
            return ParseFromContent(content);
        }

        public static TJob[] ParseFromFile<TJob>(string filePath) where TJob : Job
        {
            string content = File.ReadAllText(filePath);
            return ParseFromContent<TJob>(content);
        }

        public static Job[] ParseFromResource(string filePath)
        {
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filePath);
            using StreamReader reader = new StreamReader(stream);
            string content = reader.ReadToEnd();
            reader.Dispose();
            stream.Dispose();

            return ParseFromContent(content);
        }

        public static TJob[] ParseFromResource<TJob>(string filePath) where TJob : Job
        {
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filePath);
            using StreamReader reader = new StreamReader(stream);
            string content = reader.ReadToEnd();
            reader.Dispose();
            stream.Dispose();

            return ParseFromContent<TJob>(content);
        }
    }
}
