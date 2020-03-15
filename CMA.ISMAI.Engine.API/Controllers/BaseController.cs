using CMA.ISMAI.Engine.API.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace CMA.ISMAI.Engine.API.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult OkAction()
        {
            return Ok(
                        JsonConvert.SerializeObject(new Response(false, DateTime.Now, null)));
        }
        public IActionResult BadResultAction(string message)
        {
            return BadRequest(
                        JsonConvert.SerializeObject(new Response(true, DateTime.Now, message)));
        }       
    }
}