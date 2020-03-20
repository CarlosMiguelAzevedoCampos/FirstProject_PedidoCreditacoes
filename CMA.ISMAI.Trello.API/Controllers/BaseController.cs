using Microsoft.AspNetCore.Mvc;

namespace CMA.ISMAI.Trello.API.Controllers
{
    public class BaseController : Controller
    {
        protected new IActionResult Response(bool resultStatus, object result = null)
        {
            if (resultStatus)
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
                errors =result
            });
        }
    }
}