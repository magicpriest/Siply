﻿using System;
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
            if (parser == null) throw new ArgumentNullException("parser");

            return from leading in SP.Many()
                   from item in parser
                   from trailing in SP.Many()
                   select item;
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


        static readonly Parser<IReadOnlyDictionary<string, string>> HeaderFieldsParser = (from fieldKey in Identifier.Word()
                                                                                          from separator in Parse.Char(':').Word()
                                                                                          from fieldValue in FieldValue
                                                                                          select new { fieldKey, fieldValue }).Many()
                                                                                          .Select(fs => {
                                                                                              var dictionary = new Dictionary<string, string>();
                                                                                              foreach (var f in fs)
                                                                                              {
                                                                                                  dictionary.Add(f.fieldKey, f.fieldValue);
                                                                                              }

                                                                                              return dictionary;
                                                                                          });

        public static Parser<Request> RequestParser = from method in MethodParser.Word()
                                                      from uri in UriParser.Word()
                                                      from version in Parse.String("SIP/2.0").Token()
                                                      from fields in HeaderFieldsParser
                                                      from cflf in CRLF
                                                      from body in Parse.AnyChar.Many().Select(cs => cs.ToArray())
                                                      select new Request(method, uri, fields, body);

        //public static 

    }
}