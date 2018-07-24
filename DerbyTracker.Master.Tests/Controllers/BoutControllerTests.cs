using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
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
            var controller = new BoutController(new MockBoutDataService(), new BoutRunnerService());
            Assert.NotNull(controller);
        }

        [Fact]
        public void ListReturns()
        {
            var controller = new BoutController(new MockBoutDataService(), new BoutRunnerService());
            var list = controller.List();
            Assert.NotNull(list);
        }

        [Fact]
        public void SaveDoesntExplode()
        {
            var controller = new BoutController(new MockBoutDataService(), new BoutRunnerService());
            controller.Save(new Common.Entities.BoutData());
        }

        [Fact]
        public void LoadReturnsABout()
        {
            var controller = new BoutController(new MockBoutDataService(), new BoutRunnerService());
            var bout = controller.Load(MockBoutDataService.EmptyBoutId);
            Assert.Equal(MockBoutDataService.EmptyBoutId, bout.BoutId);
        }
    }
}
