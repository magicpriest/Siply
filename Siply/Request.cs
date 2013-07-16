using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siply.SIP
{
    public enum RequestMethod
    {
        Register,
        Invite,
        Ack,
        Bye
    }

    public class Request
    {
        private readonly RequestMethod _method;
        private readonly Uri _uri;
        private readonly Dictionary<string, List<string>> _fields;
        private readonly char[] _body;

        public Request(RequestMethod method, Uri uri, Dictionary<string, List<string>> fields, char[] body) 
        {
            _method = method;
            _uri = uri;
            _fields = fields;
            _body = body;
        }

        public RequestMethod Method
        {
            get { return _method; }
        }

        public Uri Uri
        {
            get
            { return _uri; }
        }

        public Dictionary<string, List<string>> HeaderFields
        {
            get { return _fields; }
        }

        public char[] Body
        {
            get { return _body; }
        }

        public override string ToString()
        {
            string methodLine = Method + ' ' + Uri.ToString() + " SIP/2.0\r\n";
            string headerFields = String.Concat(HeaderFields.Select(kv =>
            {
                return String.Concat(kv.Value.Select(v => kv.Key + ": " + v + Constants.CRLF));
            }));
            

            string request = methodLine + headerFields;
            return  request;
        }
    }
}
