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

        public Request(RequestMethod method, Uri uri) 
        {
            _method = method;
            _uri = uri;
        }

        public RequestMethod Method
        {
            get 
            {
                return _method;
            }
        }

        public Uri Uri
        {
            get
            {
                return _uri;
            }
        }

    }
}
