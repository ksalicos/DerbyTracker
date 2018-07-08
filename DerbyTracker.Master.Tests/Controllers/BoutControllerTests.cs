using DerbyTracker.Master.Controllers;
using Xunit;

namespace DerbyTracker.Master.Tests.Controllers
{
    public class BoutControllerTests
    {
        //This would mostly be a test of the Mock, just adding basic smoke detectors.

        [Fact]
        public void BoutControllerCanBeConstructed()
        {
            var controller = new BoutController(new MockBoutFileService());
            Assert.NotNull(controller);
        }

        [Fact]
        public void ListReturns()
        {
            var controller = new BoutController(new MockBoutFileService());
            var list = controller.List();
            Assert.NotNull(list);
        }

        [Fact]
        public void SaveDoesntExplode()
        {
            var controller = new BoutController(new MockBoutFileService());
            controller.Save(new Common.Entities.Bout());
        }

        [Fact]
        public void LoadReturnsABout()
        {
            var controller = new BoutController(new MockBoutFileService());
            var bout = controller.Load(MockBoutFileService.EmptyBoutId);
            Assert.Equal(MockBoutFileService.EmptyBoutId, bout.BoutId);
        }
    }
}
