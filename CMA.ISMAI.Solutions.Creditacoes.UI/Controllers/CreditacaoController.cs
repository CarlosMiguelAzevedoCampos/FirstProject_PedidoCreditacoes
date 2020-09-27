using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMA.ISMAI.Solutions.Creditacoes.UI.Controllers
{
    public class CreditacaoController : Controller
    {
        private readonly ITrelloService _trelloService;
        private readonly ILog _log;
        public CreditacaoController(ITrelloService trelloService, ILog log)
        {
            _trelloService = trelloService;
            _log = log;
        }
        public IActionResult Create()
        {
            return View();
        }

        // POST: Creditacao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreditacaoDto creditacaoDto)
        {
            _log.Info("New creditation process has submited");
            if (ModelState.IsValid)
            {
                bool value = _trelloService.CreateTrelloCard(creditacaoDto);
                if (!value)
                {
                    _log.Fatal("Card or proccess has not created in trello while creating a new credition!");
                    TempData["Process_status"] = "Ocorreu um erro!, porfavor, contacte o IT";
                    return RedirectToAction("Create", "Creditacao");
                }
                TempData["Process_status"] = "Processo Criado com Sucesso!";
                return RedirectToAction("Create", "Creditacao");
            }
            _log.Fatal("WorkFlow has not deployed!, an error happend!");
            TempData["Process_status"] = "Ocorreu um erro!, porfavor, verifique os seus campos";
            return RedirectToAction("Create", "Creditacao");
        }
    }
}
