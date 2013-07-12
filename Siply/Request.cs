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
        private readonly IReadOnlyDictionary<string, string> _fields;

        public Request(RequestMethod method, Uri uri, IReadOnlyDictionary<string, string> fields) 
        {
            _method = method;
            _uri = uri;
            _fields = fields;
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

        public IReadOnlyDictionary<string, string> HeaderFields
        {
            get { return _fields; }
        }

    }
}
