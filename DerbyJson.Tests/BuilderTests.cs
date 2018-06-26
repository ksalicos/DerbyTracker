using Xunit;

namespace DerbyJson.Tests
{
    public class BuilderTests
    {
        [Fact]
        public void RootDoesNotExplode()
        {
            var builder = new DerbyJsonBuilder();
            builder.Get();
        }
    }
}
