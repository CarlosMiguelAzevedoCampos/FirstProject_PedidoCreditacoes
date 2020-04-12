using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace CMA.ISMAI.EndToEndTests.Feature
{
    [Binding]
    public class CreditacaoSteps
    {
        IWebDriver webDriver;
        public CreditacaoSteps()
        {
            webDriver = new ChromeDriver(@"C:\Users\Carlos Campos\Downloads\chromedriver_win32");
        }
        [When(@"Navigate to Criar Novo Processo")]
        public void WhenNavigateToCriarNovoProcesso()
        {
            webDriver.Navigate().GoToUrl("https://localhost:5005/Creditacao/Create");
        }

        [When(@"Enter the form asking details")]
        public void WhenEnterTheFormAskingDetails()
        {
            webDriver.FindElement(By.Id("StudentName")).SendKeys("Carlos Miguel Castro Azevedo Campos");
            webDriver.FindElement(By.Id("InstituteName")).SendKeys("ISMAI");
            webDriver.FindElement(By.Id("CourseName")).SendKeys("Informática");
            webDriver.FindElement(By.Id("Documents")).SendKeys("https://abola.pt");
        }
        
        [When(@"Click on the submit button")]
        public void WhenClickOnTheSubmitButton()
        {
            webDriver.FindElement(By.Id("submitCreditacao")).Click();
        }

        [Then(@"The result should appear in the screen")]
        public void ThenTheResultShouldAppearInTheScreen()
        {
            Assert.AreEqual(webDriver.FindElement(By.Id("creditacaoSubmit_Result")).Text, "Process deployed!");
            webDriver.Close();
            webDriver.Dispose();
        }
    }
}
