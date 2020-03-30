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
        public CreditacaoController(ITrelloService trelloService, IWorkFlowService workFlowService)
        {
            _trelloService = trelloService;
            _workFlowService = workFlowService;
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
            if (ModelState.IsValid)
            {
                string value = _trelloService.CreateTrelloCard(creditacaoDto);
                if (string.IsNullOrEmpty(value))
                    return BadRequest(RedirectToAction(nameof(Create)));

                // Camunda para iniciar o processo...
                bool result = _workFlowService.CreateWorkFlowProcess(creditacaoDto, value);
                if (result)
                    return Ok();
            }
            return BadRequest();
        }
    }
}
