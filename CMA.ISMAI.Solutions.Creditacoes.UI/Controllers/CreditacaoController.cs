using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CMA.ISMAI.Solutions.Creditacoes.UI.Controllers
{
    public class CreditacaoController : Controller
    {
        private readonly ITrelloService _trelloService;
        private readonly IWorkFlowService _workFlowService;
        private readonly ILog _log;
        public CreditacaoController(ITrelloService trelloService, IWorkFlowService workFlowService, ILog log)
        {
            _trelloService = trelloService;
            _workFlowService = workFlowService;
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
                string value = _trelloService.CreateTrelloCard(creditacaoDto);
                if (string.IsNullOrEmpty(value))
                {
                    _log.Fatal("Card has not created in trello while creating a new credition!");
                    return BadRequest(Create());
                }
                // Camunda para iniciar o processo...
                bool result = _workFlowService.CreateWorkFlowProcess(creditacaoDto, value);
                if (result)
                {
                    _log.Info("Card and WorkFlow deployed!, new creditation created!");
                    return Ok(RedirectToAction("Index", "Home"));
                }
            }
            _log.Fatal("WorkFlow has not deployed!, an error happend!");
            return BadRequest(Create());
        }
    }
}
