using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Test2;

[ApiController]
[Route("[controller]/[action]")]
public class UserController: ControllerBase
{
    [HttpGet]
    public IActionResult GetToken(){
        return Ok("Testing.. ");
    }

}
