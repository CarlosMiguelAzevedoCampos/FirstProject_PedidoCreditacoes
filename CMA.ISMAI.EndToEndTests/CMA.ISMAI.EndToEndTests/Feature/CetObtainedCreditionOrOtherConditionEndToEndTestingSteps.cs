using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using TechTalk.SpecFlow;

namespace CMA.ISMAI.EndToEndTests.Feature
{
    [Binding]
    public class CetObtainedCreditionOrOtherConditionEndToEndTestingSteps
    {
        private readonly IWebDriver webDriver;
        private string _cardName;
        private readonly string _instituteName;
        private readonly string _courseName;
        private readonly string _documents;
        private readonly string _studentName;
        private readonly By _cardOnTheBoard;
        private readonly By _titleTextArea;
        private readonly By _completeCard;
        private readonly By _clickToMoveTheCard;
        private readonly By _chooseListDistination;
        private readonly By _titleTextAreaIsEditing;

        public CetObtainedCreditionOrOtherConditionEndToEndTestingSteps()
        {
            ChromeOptions options = new ChromeOptions();
            // Onde se encontra a informação do google chrome
            options.AddArguments(@"user-data-dir=C:\Users\Carlos Campos\AppData\Local\Google\Chrome\User Data");
            // Onde se encontra o web driver
            webDriver = new ChromeDriver(@"C:\Users\Carlos Campos\Downloads\google", options);
            string randomGuid = Guid.NewGuid().ToString();
            _cardName = $"ISMAI - Informática - Generated By testing Framework - {randomGuid}";
            _instituteName = "ISMAI";
            _courseName = "Informática";
            _studentName = $"Generated By testing Framework - {randomGuid}";
            _documents = "https://abola.pt";
            _cardOnTheBoard = By.XPath($"//span[contains(@class,'list-card-title js-card-name')][contains(text(),'{_cardName}')]");
            _completeCard = By.XPath("//a[@class='card-detail-badge-due-date-complete-box js-card-detail-due-date-badge-complete js-details-toggle-due-date-complete']");
            _clickToMoveTheCard = By.XPath("//a[@class='button-link js-move-card']");
            _titleTextArea = By.XPath("//textarea[@class='mod-card-back-title js-card-detail-title-input']");
            _chooseListDistination = By.XPath("//a[@class='button-link js-suggested-move']");
            _titleTextAreaIsEditing = By.XPath("//textarea[@class='mod-card-back-title js-card-detail-title-input is-editing']");
        }

        [When(@"I navigate to Criar Processo page")]
        public void WhenINavigateToCriarProcessoPage()
        {
            // Endereço onde se encontra o website
            webDriver.Navigate().GoToUrl("https://localhost:5005/Creditacao/Create");
        }

        [When(@"I fill in the necessary form information")]
        public void WhenIFillInTheNecessaryFormInformation()
        {
            webDriver.FindElement(By.Id("StudentName")).SendKeys(_studentName);
            webDriver.FindElement(By.Id("InstituteName")).SendKeys(_instituteName);
            webDriver.FindElement(By.Id("CourseName")).SendKeys(_courseName);
            webDriver.FindElement(By.Id("IsCetOrOtherCondition")).Click();
            webDriver.FindElement(By.Id("Documents")).SendKeys(_documents);
        }
        
        [When(@"Click on the submit button")]
        public void WhenClickOnTheSubmitButton()
        {
            webDriver.FindElement(By.Id("submitCreditacao")).Click();
        }

        [Then(@"A sucess message should come up on the screen")]
        public void ThenASucessMessageShouldComeUpOnTheScreen()
        {
            Assert.AreEqual(webDriver.FindElement(By.Id("creditacaoSubmit_Result")).Text, "Processo Criado com Sucesso!");
        }

        [Then(@"I need go to the course coordinators board on Trello")]
        public void ThenINeedGoToTheCourseCoordinatorsBoardOnTrello()
        {
            webDriver.Navigate().GoToUrl("https://trello.com/b/EddKNgdn/cordenadores-de-curso");
        }

        [Then(@"Put the created card on this board in done")]
        public void ThenPutTheCreatedCardOnThisBoardInDone()
        {
            WaitTillCardisDisplayedInTheBoard();
            Thread.Sleep(5000);
            webDriver.FindElement(_cardOnTheBoard).Click();
            Thread.Sleep(5000);
            webDriver.FindElement(_completeCard).Click();
            Thread.Sleep(5000);
            webDriver.FindElement(_clickToMoveTheCard).Click();
            Thread.Sleep(5000);
            string randomGuid = Guid.NewGuid().ToString();
            webDriver.FindElement(_titleTextArea).Clear();
            webDriver.FindElement(_titleTextAreaIsEditing).SendKeys($"ISMAI - Informática - Generated By testing Framework - {randomGuid}");
            Thread.Sleep(5000);
            webDriver.FindElement(_chooseListDistination).Click();
        }


        [Then(@"I need go to the course coordinators board on Trello again")]
        public void ThenINeedGoToTheCourseCoordinatorsBoardOnTrelloAgain()
        {
            webDriver.Navigate().GoToUrl("https://trello.com/b/EddKNgdn/cordenadores-de-curso");
        }

        [Then(@"Put the created card on this board in done, this card represent jury validation")]
        public void ThenPutTheCreatedCardOnThisBoardInDoneThisCardRepresentJuryValidation()
        {
            WaitTillCardisDisplayedInTheBoard();
            Thread.Sleep(5000);
            webDriver.FindElement(_cardOnTheBoard).Click();
            Thread.Sleep(5000);
            webDriver.FindElement(_completeCard).Click();
            Thread.Sleep(5000);
            webDriver.FindElement(_clickToMoveTheCard).Click();
            Thread.Sleep(5000);
            webDriver.FindElement(_chooseListDistination).Click();
            Thread.Sleep(5000);
        }
        
        [Then(@"I need go to the cientific concil board on trello")]
        public void ThenINeedGoToTheCientificConcilBoardOnTrello()
        {
            webDriver.Navigate().GoToUrl("https://trello.com/b/3ZEvrC5A/concelho-cientifico");
        }

        [Then(@"Put the created card on cientific concil board in done")]
        public void ThenPutTheCreatedCardOnCientificConcilBoardInDone()
        {
            WaitTillCardisDisplayedInTheBoard();
            Thread.Sleep(5000);
            webDriver.FindElement(_cardOnTheBoard).Click();
            Thread.Sleep(5000);
            webDriver.FindElement(_completeCard).Click();
            Thread.Sleep(5000);
            webDriver.FindElement(_clickToMoveTheCard).Click();
            Thread.Sleep(5000);
            webDriver.FindElement(_chooseListDistination).Click();
            Thread.Sleep(5000);
            webDriver.Close();
            webDriver.Dispose();
        }


        private void WaitTillCardisDisplayedInTheBoard()
        {
            bool elementDisplayed = false;
            while (!elementDisplayed)
            {
                try
                {
                    var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(40));
                    wait.Until(drv => drv.FindElement(_cardOnTheBoard));
                    elementDisplayed = webDriver.FindElement(_cardOnTheBoard).Displayed;
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}
