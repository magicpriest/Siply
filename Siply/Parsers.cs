using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace Siply.SIP
{
    public static class Parsers
    {
        static bool IsNewLine(char c)
        {
            return UnicodeCategoryPredicate(UnicodeCategory.LineSeparator)(c) || c == '\n';
        }

        static Predicate<char> UnicodeCategoryPredicate(UnicodeCategory category)
        {
            return c => Char.GetUnicodeCategory(c) == category;
        }

        static readonly Parser<char> SP = Parse.Char(UnicodeCategoryPredicate(UnicodeCategory.SpaceSeparator), "space");
        static readonly Parser<char> LF = Parse.Char(UnicodeCategoryPredicate(UnicodeCategory.LineSeparator), "unicode line separator").Or(Parse.Char('\n'));
        static readonly Parser<IEnumerable<char>> CRLF = Parse.String("\r\n");

        static readonly Parser<string> Identifier = Parse.LetterOrDigit.Or(Parse.Char('-')).Or(Parse.Char('_')).AtLeastOnce().Text();

        // extension
        public static Parser<T> Word<T>(this Parser<T> parser)
        {
            return parser.Contained(SP.Many(), SP.Many());
        }


        static Parser<RequestMethod> CreateMethodParser(string methodName, RequestMethod method)
        {
            return Parse.String(methodName).Return(method);
        }

        static readonly Parser<RequestMethod> Invite = CreateMethodParser("INVITE", RequestMethod.Invite);
        static readonly Parser<RequestMethod> Register = CreateMethodParser("REGISTER", RequestMethod.Register);
        static readonly Parser<RequestMethod> Ack = CreateMethodParser("ACK", RequestMethod.Ack);
        static readonly Parser<RequestMethod> Bye = CreateMethodParser("BYE", RequestMethod.Bye);

        static readonly Parser<RequestMethod> MethodParser = Invite.Or(Register).Or(Ack).Or(Bye);

        static readonly Parser<string> SchemeParser = Parse.String("sip:").Text();
        static readonly Parser<string> UserParser = Parse.LetterOrDigit.AtLeastOnce().Text();
        static readonly Parser<string> HostParser = Parse.LetterOrDigit.Or(Parse.Char('.')).Or(Parse.Char('-')).AtLeastOnce().Text();

        static readonly Parser<Uri> UriParser = from uri in SchemeParser
                                                from user in UserParser
                                                from at in Parse.Char('@')
                                                from host in HostParser
                                                select new Uri(user, host);

        static readonly Parser<string> FieldValue = from leading in SP.Many()
                                                    from fieldValue in Parse.CharExcept(IsNewLine, "field value").AtLeastOnce().Text()
                                                    from linefeed in LF
                                                    select fieldValue;

        static readonly Parser<Dictionary<string, List<string>>> HeaderFieldsParser = (from fieldKey in Identifier.Word()
                                                                              from separator in Parse.Char(':').Word()
                                                                              from fieldValue in FieldValue
                                                                              select new { fieldKey, fieldValue }).Many()
                                                                                          .Select(fs =>
                                                                                          {
                                                                                              var dict = new Dictionary<string, List<string>>();
                                                                                              foreach(var f in fs) 
                                                                                              {
                                                                                                  if (dict.ContainsKey(f.fieldKey))
                                                                                                  {
                                                                                                      dict[f.fieldKey].Add(f.fieldValue);
                                                                                                  }
                                                                                                  else
                                                                                                  {
                                                                                                      dict[f.fieldKey] = new List<string>() { f.fieldValue };
                                                                                                  }
                                                                                              }
                                                                                              return dict;
                                                                                          });

        public static readonly Parser<Request> RequestParser = from method in MethodParser.Word().Named("Method")
                                                               from uri in UriParser.Word().Named("Uri")
                                                               from version in Parse.String("SIP/2.0").Token().Named("Protocol\version")
                                                               from fields in HeaderFieldsParser.Named("Header fields")
                                                               from crlf in CRLF.Named("Emptry line separator")
                                                               from body in Parse.AnyChar.Many().Select(cs => cs.ToArray())
                                                               select new Request(method, uri, fields, body);


        static readonly Parser<int> CodeParser = Parse.Digit.Repeat(3).Text().Select(Convert.ToInt32);
        static readonly Parser<string> ReasonParser = Parse.CharExcept(c => IsNewLine(c) || c == '\r', "phrase except new line").Many().Text();

        public static readonly Parser<Response> ResponseParser = from version in Parse.String("SIP/2.0").Word().Named("Protocol/Version")
                                                                 from code in CodeParser.Word().Named("Status code")
                                                                 from reason in ReasonParser.Token().Named("Reason phrase")
                                                                 from fields in HeaderFieldsParser.Named("Response fields")
                                                                 from crlf in CRLF.Named("Empty line separator")
                                                                 from body in Parse.AnyChar.Many().Select(cs => cs.ToArray())
                                                                 select new Response(code, reason, fields, null);


    }
}