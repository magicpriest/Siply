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
        private readonly ILookup<string, string> _fields;
        private readonly char[] _body;

        public Request(RequestMethod method, Uri uri, ILookup<string, string> fields, char[] body) 
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

        public ILookup<string, string> HeaderFields
        {
            get { return _fields; }
        }

        public char[] Body
        {
            get { return _body; }
        }
    }
}
