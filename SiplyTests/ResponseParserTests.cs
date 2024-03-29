﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Siply.SIP;
using Sprache;

namespace SiplyTests
{
    [TestClass]
    public class ResponseParserTests
    {
        [TestMethod]
        public void TestMethod2()
        {
            var input = @"SIP/2.0 200 OK
Via: SIP/2.0/UDP server10.biloxi.com;branch=z9hG4bKnashds8;received=192.0.2.3
Via: SIP/2.0/UDP bigbox3.site3.atlanta.com;branch=z9hG4bK77ef4c2312983.1;received=192.0.2.2
Via: SIP/2.0/UDP pc33.atlanta.com;branch=z9hG4bK776asdhds;received=192.0.2.1
To: Bob <sip:bob@biloxi.com>;tag=a6c85cf
From: Alice <sip:alice@atlanta.com>;tag=1928301774
Call-ID: a84b4c76e66710@pc33.atlanta.com
CSeq: 314159 INVITE
Contact: <sip:bob@192.0.2.4>
Content-Type: application/sdp
Content-Length: 131

";

            var response = Parsers.ResponseParser.Parse(input);
            var via = response.HeaderFields["Via"].ToArray();
            Assert.AreEqual(response.Code, 200);
        }
    }
}
