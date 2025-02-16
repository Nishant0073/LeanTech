using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Test2;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class TestController: ControllerBase
{
    [HttpGet]
    public IActionResult GetToken(){
        return Ok("Testing.. ");
    }
}
