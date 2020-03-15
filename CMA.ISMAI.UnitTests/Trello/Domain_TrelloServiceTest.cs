using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.Domain.Interface;
using CMA.ISMAI.Trello.Domain.Model;
using CMA.ISMAI.Trello.Domain.Service;
using CMA.ISMAI.Trello.Engine.Interface;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Engine.Domain
{
    public class Domain_TrelloServiceTest
    {
        [Fact]
        public void TrelloService_AddCard_ShouldReturnFalseBecauseOfBadFormat()
        {
            var engineMock = new Mock<ITrello>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();
            engineMock.Setup(x => x.AddCard(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(Task.FromResult(string.Empty));

            ITrelloService engineService = new TrelloService(engineMock.Object, logMock.Object);

            string result = engineService.AddCard(new Card("", DateTime.Now, "")).Result;
            Assert.Empty(result);
        }

        [Fact]
        public void TrelloService_AddCard_ShouldReturnTrue()
        {
            var engineMock = new Mock<ITrello>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();
            engineMock.Setup(x => x.AddCard(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(Task.FromResult(Guid.NewGuid().ToString()));

            ITrelloService engineService = new TrelloService(engineMock.Object, logMock.Object);

            string result = engineService.AddCard(new Card("Carlos", DateTime.Now, "Insert card")).Result;
            Assert.NotEmpty(result);
        }

        [Fact]
        public void TrelloService_IsTheProcessFinished_ShouldReturnTrue()
        {
            var engineMock = new Mock<ITrello>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();
            engineMock.Setup(x => x.IsTheProcessFinished(It.IsAny<string>())).Returns(Task.FromResult(true));

            ITrelloService engineService = new TrelloService(engineMock.Object, logMock.Object);

            bool result = engineService.IsTheProcessFinished(Guid.NewGuid().ToString()).Result;
            Assert.True(result);
        }

        [Fact]
        public void TrelloService_IsTheProcessFinished_ShouldReturnNo()
        {
            var engineMock = new Mock<ITrello>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();
            engineMock.Setup(x => x.IsTheProcessFinished(It.IsAny<string>())).Returns(Task.FromResult(false));

            ITrelloService engineService = new TrelloService(engineMock.Object, logMock.Object);

            bool result = engineService.IsTheProcessFinished(Guid.NewGuid().ToString()).Result;
            Assert.False(result);
        }


    }
}
