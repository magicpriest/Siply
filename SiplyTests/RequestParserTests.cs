using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Siply.SIP;
using Sprache;

namespace SiplyTests
{
    [TestClass]
    public class RequestParserTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var input = "INVITE sip:alice@yptele.sip";
            Request request = Parsers.RequestParser.Parse(input);
            Siply.SIP.Uri uri = request.Uri;
            Assert.AreEqual(request.Method, RequestMethod.Invite);
            Assert.AreEqual(uri.User, "alice");
            Assert.AreEqual(uri.Host, "yptele.sip");
        }
    }
}
