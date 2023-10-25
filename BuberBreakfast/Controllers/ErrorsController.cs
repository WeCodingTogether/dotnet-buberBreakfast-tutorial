using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfas.Controllers;

public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        return Problem();
    }
}