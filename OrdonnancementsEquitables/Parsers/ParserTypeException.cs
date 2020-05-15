using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Parsers
{
    [System.Serializable]
    public class ParserTypeException : Exception
    {
        private static readonly string defaultMessage = "Parser's and JSON's Job Types don't match";
        public ParserTypeException() : base(defaultMessage) { }
        public ParserTypeException(string message) : base(message) { }
        public ParserTypeException(string message, Exception inner) : base(message, inner) { }
        protected ParserTypeException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
