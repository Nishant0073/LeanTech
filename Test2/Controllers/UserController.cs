using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Test2;

[ApiController]
[Route("[controller]/")]
public class UserController: ControllerBase
{
    private IUserService _userService;
    public UserController(IUserService userService)
    {
       _userService = userService; 
    }
    [HttpPost("token")]
    public async Task<IActionResult> GetToken(LoginModel model){
        if(!ModelState.IsValid){
            return BadRequest();
        }
        return Ok(await _userService.GetToken(model));
    }

}
