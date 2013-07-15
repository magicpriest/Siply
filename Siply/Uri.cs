using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace Siply.SIP
{
    public class Uri
    {
        private readonly string _scheme;
        private readonly string _user;
        private readonly string _host;

        public Uri(string uriString)
        {

        }

        public Uri(string user, string host, string scheme = "sip:")
        {
            _scheme = scheme;
            _host = host;
            _user = user;
        }

        public string User 
        {
            get 
            {
                return _user;
            }
        }

        public string Scheme
        {
            get
            {
                return _scheme;
            }
        }

        public string Host
        {
            get
            {
                return _host;
            }
        }

        public override string ToString()
        {
            return Scheme + ":" + User + "@" + Host;
        }
    }
}
