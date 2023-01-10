using OpenTokSDK.Exception;
using OpenTokSDK.Render;
using Xunit;

namespace OpenTokSDKTest.Render
{
    public class ListRendersRequestTests
    {
        [Fact]
        public void ListRendersRequest_ShouldThrowOpenTokException_GivenCountIsHigherThanThreshold()
        {
            void Act() => new ListRendersRequest(1001);
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(ListRendersRequest.CountExceeded, exception.Message);
        }

        [Fact]
        public void ListRendersRequest_ShouldThrowOpenTokException_GivenCountIsNegative()
        {
            void Act() => new ListRendersRequest(-1);
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(ListRendersRequest.NegativeCount, exception.Message);
        }

        [Fact]
        public void ListRendersRequest_ShouldThrowOpenTokException_GivenOffsetIsNegative()
        {
            void Act() => new ListRendersRequest(-1, 1);
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(ListRendersRequest.NegativeOffset, exception.Message);
        }

        [Theory]
        [InlineData(0, 0)]
        public void ListRendersRequest_ShouldReturnInstance(int offset, int count)
        {
            var request = new ListRendersRequest(offset, count);
            Assert.Equal(offset, request.Offset);
            Assert.Equal(count, request.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1000)]
        public void ListRendersRequest_ShouldReturnInstance_GivenOnlyCountIsProvided(int count)
        {
            var request = new ListRendersRequest(count);
            Assert.Null(request.Offset);
            Assert.Equal(count, request.Count);
        }

        [Fact]
        public void ListRendersRequest_ShouldHaveDefaultCount()
        {
            var request = new ListRendersRequest();
            Assert.Null(request.Offset);
            Assert.Equal(50, request.Count);
        }
    }
}