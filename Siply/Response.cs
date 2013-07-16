using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siply.SIP
{
    public class Response
    {
        readonly int _code;
        readonly string _reason;
        readonly Dictionary<string, List<string>> _fields;
        readonly char[] _body;

        public Response(int code, string reason, Dictionary<string, List<string>> fields, char[] body)
        {
            _code = code;
            _reason = reason;
            _fields = fields;
            _body = body;
        }

        public int Code
        {
            get { return _code; }
        }

        public string Reason
        {
            get { return _reason; }
        }

        public Dictionary<string, List<string>> HeaderFields
        {
            get { return _fields; }
        }

        public char[] Body
        {
            get { return _body; }
        }
    }
}
