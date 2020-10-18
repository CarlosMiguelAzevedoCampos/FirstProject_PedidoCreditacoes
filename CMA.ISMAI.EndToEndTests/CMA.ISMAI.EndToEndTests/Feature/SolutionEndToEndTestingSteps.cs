﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using TechTalk.SpecFlow;

namespace CMA.ISMAI.EndToEndTests
{
    [Binding]
    public class SolutionEndToEndTestingSteps
    {
        private readonly IWebDriver webDriver;
        private readonly string _cardName;
        private readonly string url;
        private readonly string _instituteName;
        private readonly string _courseName;
        private readonly string _documents;
        private readonly string _studentName;
        private readonly By _cardOnTheBoard;
        private readonly By _completeCard;
        private readonly By _clickToMoveTheCard;
        private readonly By _chooseListDistination;

        public SolutionEndToEndTestingSteps()
        {
            ChromeOptions options = new ChromeOptions();
            // De seguida, devemos colocar o sitio onde temos a informação sobre o nosso Google Chrome
            options.AddArguments(@"user-data-dir=C:\Users\Carlos Campos\AppData\Local\Google\Chrome\User Data");
            // De seguida, devemos colocar onde temos o chrome driver no nosso computador
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
            _chooseListDistination = By.XPath("//a[@class='button-link js-suggested-move']");
            url = "https://localhost:5005/Creditacao/Create";
        }
        [When(@"I navigate to Criar Processo")]
        public void WhenINavigateToCriarProcesso()
        {
            // Devemos colocar o Endereço onde o website se encontra
            webDriver.Navigate().GoToUrl(url);
        }

        [When(@"I fill in the necessary information")]
        public void WhenIFillInTheNecessaryInformation()
        {
            webDriver.FindElement(By.Id("StudentName")).SendKeys(_studentName);
            webDriver.FindElement(By.Id("InstituteName")).SendKeys(_instituteName);
            webDriver.FindElement(By.Id("CourseName")).SendKeys(_courseName);
            webDriver.FindElement(By.Id("Documents")).SendKeys(_documents);
        }

        [When(@"Click on the submit button to crate the process")]
        public void WhenClickOnTheSubmitButtonToCrateTheProcess()
        {
            webDriver.FindElement(By.Id("submitCreditacao")).Click();
        }

        [Then(@"A sucess message should appear on the screen")]
        public void ThenASucessMessageShouldAppearOnTheScreen()
        {
            Assert.AreEqual(webDriver.FindElement(By.Id("creditacaoSubmit_Result")).Text, "Processo Criado com Sucesso!");
        }

        [Then(@"I must go to the course coordinators board on Trello")]
        public void ThenIMustGoToTheCourseCoordinatorsBoardOnTrello()
        {
            webDriver.Navigate().GoToUrl("https://trello.com/b/EddKNgdn/cordenadores-de-curso");
        }

        [Then(@"Put the created card on the scientific council board in done")]
        public void ThenPutTheCreatedCardOnTheScientificCouncilBoardInDone()
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

        [Then(@"I must go to the departament directors board on trello")]
        public void ThenIMustGoToTheDepartamentDirectorsBoardOnTrello()
        {
            webDriver.Navigate().GoToUrl("https://trello.com/b/KmT3OMLf/diretores-de-departamento");
        }

        [Then(@"Put the created card on the departament directors board in done")]
        public void ThenPutTheCreatedCardOnTheDepartamentDirectorsBoardInDone()
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

        [Then(@"I must go to the cientific concil board on trello")]
        public void ThenIMustGoToTheCientificConcilBoardOnTrello()
        {
            webDriver.Navigate().GoToUrl("https://trello.com/b/3ZEvrC5A/concelho-cientifico");
        }

        [Then(@"Put the created card on the cientific concil board in done")]
        public void ThenPutTheCreatedCardOnTheCientificConcilBoardInDone()
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

        public void WaitTillCardisDisplayedInTheBoard()
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