using OpenTokSDK;
using OpenTokSDK.Exception;
using Xunit;

namespace OpenTokSDKTest
{
    public class BroadcastBitrateTest
    {
        [Fact]
        public void Bitrate_ShouldHaveDefaultValue() => 
            Assert.Equal(2000000, new BroadcastBitrate().Bitrate);

        [Fact]
        public void Bitrate_ShouldAllowMaximumValue()=> 
            Assert.Equal(2000000, new BroadcastBitrate(2000000).Bitrate);
        
        [Fact]
        public void Bitrate_ShouldAllowMinimumValue()=> 
            Assert.Equal(400000, new BroadcastBitrate(400000).Bitrate);

        [Theory]
        [InlineData(399999)]
        [InlineData(2000001)]
        public void Bitrate_ShouldThrowException_GivenValueIsOutsideRange(int invalidBitrate)
        {
            var exception = Assert.Throws<OpenTokArgumentException>(() => new BroadcastBitrate(invalidBitrate));
            Assert.Equal("Bitrate value must be between 400000 and 2000000.", exception.Message);
        }
    }
}