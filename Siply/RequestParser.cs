using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace Siply.SIP
{
    public static partial class Parsers
    {

        static class MethodParsers
        {
            static Parser<RequestMethod> CreateMethodParser(string methodName, RequestMethod method)
            {
                return Parse.String(methodName).Return(method);
            }

            public static readonly Parser<RequestMethod> Invite = CreateMethodParser("INVITE", RequestMethod.Invite);
            public static readonly Parser<RequestMethod> Register = CreateMethodParser("REGISTER", RequestMethod.Register);
            public static readonly Parser<RequestMethod> Ack = CreateMethodParser("ACK", RequestMethod.Ack);
            public static readonly Parser<RequestMethod> Bye = CreateMethodParser("BYE", RequestMethod.Bye);

            public static readonly Parser<RequestMethod> Method = Invite.Or(Register).Or(Ack).Or(Bye).Then(t => Parse.WhiteSpace.AtLeastOnce().Return(t));
        }

        static class UriParser
        {
            static readonly Parser<string> SchemeParser = Parse.String("sip:").Text();
            static readonly Parser<string> UserParser = Parse.LetterOrDigit.AtLeastOnce().Text();
            static readonly Parser<string> HostParser = Parse.LetterOrDigit.Or(Parse.Char('.')).AtLeastOnce().Text();

            public static readonly Parser<Uri> Instance = from uri in SchemeParser
                                                           from user in UserParser
                                                           from at in Parse.Char('@')
                                                           from host in HostParser
                                                           select new Uri(user, host);
        }


        public static Parser<Request> RequestParser = from method in MethodParsers.Method.Token()
                                                      from uri in UriParser.Instance.Token().End()
                                                      select new Request(method, uri);
    }
}
