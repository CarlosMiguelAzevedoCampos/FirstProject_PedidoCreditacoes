using CMA.ISMAI.Core;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.FileReader.Interfaces;
using CMA.ISMAI.Trello.FileReader.Services;
using Moq;
using Xunit;

namespace CMA.ISMAI.UnitTests.Trello.FileReader
{
    public class FileReader_Tests
    {
        [Fact(DisplayName = "Obtain user by course and institute")]
        [Trait("FileReader", "Read the file information")]
        private void FileReader_ReturnUserNameForTheCard_ObtainUserInformation()
        {
            var logMock = new Mock<ILog>();
            IFileReader fileReaderService = new FileReaderService(logMock.Object);
            string userInfo = fileReaderService.ReturnUserNameForTheCard("ISMAI", "Informática", 0, BaseConfiguration.ReturnSettingsValue("FileReader", "Path"));
            Assert.Equal("carloscampos77", userInfo);
        }

        [Fact(DisplayName = "Fail to find the information in the file. Page dosen't exist")]
        [Trait("FileReader", "Read the file information")]
        private void FileReader_ReturnUserNameForTheCard_FailObtainUserInformation_PageError()
        {
            var logMock = new Mock<ILog>();
            IFileReader fileReaderService = new FileReaderService(logMock.Object);
            string userInfo = fileReaderService.ReturnUserNameForTheCard("ISMAI", "Informática", 4, BaseConfiguration.ReturnSettingsValue("FileReader", "Path"));
            Assert.Empty(userInfo);
        }

        [Fact(DisplayName = "Fail to find the information in the file. Information dosen't exist")]
        [Trait("FileReader", "Read the file information")]
        private void FileReader_ReturnUserNameForTheCard_FailObtainUserInformation()
        {
            var logMock = new Mock<ILog>();
            IFileReader fileReaderService = new FileReaderService(logMock.Object);
            string userInfo = fileReaderService.ReturnUserNameForTheCard("FEUP", "Informática", 1, BaseConfiguration.ReturnSettingsValue("FileReader", "Path"));
            Assert.Empty(userInfo);
        }

        [Fact(DisplayName = "File dosent exist")]
        [Trait("FileReader", "Read the file information")]
        private void FileReader_ReturnUserNameForTheCard_FileDosentExist()
        {
            var logMock = new Mock<ILog>();
            IFileReader fileReaderService = new FileReaderService(logMock.Object);
            string userInfo = fileReaderService.ReturnUserNameForTheCard("FEUP", "Informática", 1, "");
            Assert.Empty(userInfo);
        }
    }
}
