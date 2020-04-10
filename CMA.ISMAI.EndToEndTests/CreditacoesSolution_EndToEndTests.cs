using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Xunit;

namespace CMA.ISMAI.EndToEndTests
{
    public class CreditacoesSolution_EndToEndTests
    {
        private readonly IWebDriver _driver;
        public CreditacoesSolution_EndToEndTests()
        {
            _driver = new ChromeDriver("C:/Users/Carlos Campos/Downloads/chromedriver_win32");
        }

        [Fact]
        public void Test1()
        {
            _driver.Navigate().GoToUrl("https://code-maze.com/automatic-ui-testing-selenium-asp-net-core-mvc/");
            Assert.Contains(" ", _driver.Title);
        }
    }
}
