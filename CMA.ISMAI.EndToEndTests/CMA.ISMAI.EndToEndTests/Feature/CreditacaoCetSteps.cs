using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using TechTalk.SpecFlow;

namespace CMA.ISMAI.EndToEndTests.Feature
{
    [Binding]
    public class CreditacaoCetSteps
    {
        IWebDriver webDriver;
        public CreditacaoCetSteps()
        {
            webDriver = new ChromeDriver(@"C:\Users\Carlos Campos\Downloads\chromedriver_win32");
        }
        [When(@"I navigate to Criar Novo Processo")]
        public void WhenINavigateToCriarNovoProcesso()
        {
            webDriver.Navigate().GoToUrl("https://localhost:5005/Creditacao/Create");
        }
        
        [When(@"Entered the form asking details")]
        public void WhenEnteredTheFormAskingDetails()
        {
            webDriver.FindElement(By.Id("StudentName")).SendKeys("Carlos Miguel Castro Azevedo Campos");
            webDriver.FindElement(By.Id("InstituteName")).SendKeys("ISMAI");
            webDriver.FindElement(By.Id("CourseName")).SendKeys("Informática");
            webDriver.FindElement(By.Id("IsCet")).Click();
            webDriver.FindElement(By.Id("Documents")).SendKeys("https://abola.pt");
        }
        
        [When(@"The submit button is pressed")]
        public void WhenTheSubmitButtonIsPressed()
        {
            webDriver.FindElement(By.Id("submitCreditacao")).Click();
        }
        
        [Then(@"The result of the operation should appear in the screen")]
        public void ThenTheResultOfTheOperationShouldAppearInTheScreen()
        {
            Assert.AreEqual(webDriver.FindElement(By.Id("creditacaoSubmit_Result")).Text, "Process deployed!");
            webDriver.Close();
            webDriver.Dispose();
        }
    }
}
