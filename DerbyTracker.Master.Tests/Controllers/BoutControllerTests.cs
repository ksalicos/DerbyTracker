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
            var controller = new BoutController(new MockBoutDataService());
            Assert.NotNull(controller);
        }

        [Fact]
        public void ListReturns()
        {
            var controller = new BoutController(new MockBoutDataService());
            var list = controller.List();
            Assert.NotNull(list);
        }

        [Fact]
        public void SaveDoesntExplode()
        {
            var controller = new BoutController(new MockBoutDataService());
            controller.Save(new Common.Entities.Bout());
        }

        [Fact]
        public void LoadReturnsABout()
        {
            var controller = new BoutController(new MockBoutDataService());
            var bout = controller.Load(MockBoutDataService.EmptyBoutId);
            Assert.Equal(MockBoutDataService.EmptyBoutId, bout.BoutId);
        }
    }
}
