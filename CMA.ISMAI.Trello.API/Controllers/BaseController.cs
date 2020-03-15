using CMA.ISMAI.Trello.API.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace CMA.ISMAI.Trello.API.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult OkAction(string message)
    {
        return Ok(
                    JsonConvert.SerializeObject(new Response(false, DateTime.Now, message)));
    }
    public IActionResult BadResultAction(string message)
    {
        return BadRequest(
                    JsonConvert.SerializeObject(new Response(true, DateTime.Now, message)));
    }
}
}