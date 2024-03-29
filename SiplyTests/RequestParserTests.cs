﻿using System;
using System.Collections.Generic;
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
            var input = @"INVITE sip:bob@biloxi.com SIP/2.0
Via: SIP/2.0/UDP pc33.atlanta.com;branch=z9hG4bK776asdhds
Max-Forwards: 70
To: Bob <sip:bob@biloxi.com>
From: Alice <sip:alice@atlanta.com>;tag=1928301774
Call-ID: a84b4c76e66710@pc33.atlanta.com
CSeq: 314159 INVITE
Contact: <sip:alice@pc33.atlanta.com>
Content-Type: application/sdp
Content-Length: 142

";

            Request request = Parsers.RequestParser.Parse(input);
            Siply.SIP.Uri uri = request.Uri;
            Assert.AreEqual(request.Method, RequestMethod.Invite);
            Assert.AreEqual(uri.User, "bob");
            Assert.AreEqual(uri.Host, "biloxi.com");
        }
    }
}
