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
        readonly IReadOnlyDictionary<string, string> _fields;
        readonly char[] _body;

        public Response(int code, string reason, IReadOnlyDictionary<string, string> fields, char[] body)
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

        public IReadOnlyDictionary<string, string> HeaderFields
        {
            get { return _fields; }
        }

        public char[] Body
        {
            get { return _body; }
        }
    }
}
