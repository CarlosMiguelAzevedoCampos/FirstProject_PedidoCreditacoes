using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace CMA.ISMAI.EndToEndTests.Feature
{
    [Binding]
    public class InvalidCreditacaoSteps
    {
        IWebDriver webDriver;
        public InvalidCreditacaoSteps()
        {
            // Onde se encontra o web driver
            webDriver = new ChromeDriver(@"C:\Users\Carlos Campos\Downloads\google");
        }

        [When(@"Navigate to Criar Novo Processo page")]
        public void WhenNavigateToCriarNovoProcessoPage()
        {
            // Endereço onde o website se encontra
            webDriver.Navigate().GoToUrl("https://localhost:5005/Creditacao/Create");
        }

        [When(@"Entered the information in some fields")]
        public void WhenEnteredTheInformationInSomeFields()
        {
            webDriver.FindElement(By.Id("StudentName")).SendKeys("Carlos Miguel Castro Azevedo Campos");
            webDriver.FindElement(By.Id("CourseName")).SendKeys("Informática");
            webDriver.FindElement(By.Id("Documents")).SendKeys("https://abola.pt");
        }

        [When(@"The submit button is clicked")]
        public void WhenTheSubmitButtonIsClicked()
        {
            webDriver.FindElement(By.Id("submitCreditacao")).Click();
        }
        
        [Then(@"A label with the bad result should appear")]
        public void ThenALabelWithTheBadResultShouldAppear()
        {
            Assert.AreEqual(webDriver.FindElement(By.Id("InstituteName-error")).Text, "The Nome da Instituição field is required.");
            webDriver.Close();
            webDriver.Dispose();
        }
    }
}
