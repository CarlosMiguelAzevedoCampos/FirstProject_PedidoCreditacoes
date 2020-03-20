using Microsoft.AspNetCore.Mvc;

namespace CMA.ISMAI.Engine.API.Controllers
{
    public class BaseController : Controller
    {      
        protected new IActionResult Response(bool resultCode, object result = null)
        {
            if (resultCode)
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = result
            });
        }
    }
}