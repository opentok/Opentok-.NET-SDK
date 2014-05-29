using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using OpenTokSDK;

namespace OpenTokTest
{
    public class OpenTokTest
    {
        [Fact]
        public void MyTest()
        {
            Assert.Equal(4, 2 + 2);
        }

        [Fact]
        public void InitializationTest()
        {
            var opentok = new OpenTok(123456, "1234567890abcdef1234567890abcdef1234567890");
            Assert.IsType(typeof(OpenTok), opentok);
        }
    }
}
