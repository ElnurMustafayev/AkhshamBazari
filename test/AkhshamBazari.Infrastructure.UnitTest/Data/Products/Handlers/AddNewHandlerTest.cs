using AkhshamBazari.Core.Data.Products.Repositories;
using AkhshamBazari.Infrastructure.Data.Products.Commands;
using AkhshamBazari.Infrastructure.Data.Products.Handlers;

namespace AkhshamBazari.Infrastructure.UnitTest.Data.Products.Handlers
{
    public class AddNewHandlerTest
    {
        private const string correctName = "Test";
        private const double correctPrice = 123;

        private readonly AddNewCommand correctCommand = new()
        {
            Name = correctName,
            Price = correctPrice
        };

        [Fact]
        public async Task Handle_SendCorrectProduct_Works()
        {
            var productRepositoryMock = new Mock<IProductRepository>();

            var handler = new AddNewHandler(productRepositoryMock.Object);

            await handler.Handle(this.correctCommand, CancellationToken.None);
        }

        [Theory]
        [InlineData(correctName, -10.0)]
        [InlineData("  ", correctPrice)]
        [InlineData("", correctPrice)]
        [InlineData(null, correctPrice)]
        public async Task Handle_SendIncorrectProduct_ThrowsArgumentException(string? name, double? price)
        {
            var handler = new AddNewHandler(null);

            var addNewCommand = new AddNewCommand
            {
                Name = name,
                Price = price,
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(addNewCommand, CancellationToken.None));
        }
    }
}