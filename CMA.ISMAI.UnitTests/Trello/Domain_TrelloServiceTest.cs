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
        [Theory]
        [InlineData("ISMAI - Informatica creditaçâo", "")]
        [InlineData("", "Carlos Campos")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void TrelloService_AddCard_ShouldFailBecauseOfNullOrEmptyParameters(string name, string description)
        {
            var engineMock = new Mock<ITrello>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();
            engineMock.Setup(x => x.AddCard(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(Task.FromResult(string.Empty));

            ITrelloService engineService = new TrelloService(engineMock.Object, logMock.Object);

            string result = engineService.AddCard(new Card(name, DateTime.Now, description)).Result;
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("ISMAI - Informatica creditaçâo", "Carlos Campos")]
        [InlineData("ISMAI - Multimedia creditaçâo", "Miguel Campos")]
        public void TrelloService_AddCard_ShouldReturnTrue(string name, string description)
        {
            var engineMock = new Mock<ITrello>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();
            engineMock.Setup(x => x.AddCard(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(Task.FromResult(Guid.NewGuid().ToString()));

            ITrelloService engineService = new TrelloService(engineMock.Object, logMock.Object);

            string result = engineService.AddCard(new Card(name, DateTime.Now, description)).Result;
            Assert.NotEmpty(result);
        }


        [Theory]
        [InlineData("ifh2i992h2b-asfa-1w")]
        [InlineData("cjckamrb222-we-1w")]
        public void TrelloService_IsTheProcessFinished_ShouldReturnNullOrEmptyParameters(string id)
        {
            var engineMock = new Mock<ITrello>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();
            engineMock.Setup(x => x.IsTheProcessFinished(It.IsAny<string>())).Returns(Task.FromResult(false));

            ITrelloService engineService = new TrelloService(engineMock.Object, logMock.Object);

            bool result = engineService.IsTheProcessFinished(id).Result;
            Assert.False(result);
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TrelloService_IsTheProcessFinished_ShouldReturnTrue(string id)
        {
            var engineMock = new Mock<ITrello>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();
            engineMock.Setup(x => x.IsTheProcessFinished(It.IsAny<string>())).Returns(Task.FromResult(true));

            ITrelloService engineService = new TrelloService(engineMock.Object, logMock.Object);

            bool result = engineService.IsTheProcessFinished(id).Result;
            Assert.True(result);
        }



    }
}
