using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace test1;
[ApiController]
[Route("[controller]/")]
public class UserController: ControllerBase
{
    ILogger<UserController> _logger;
    IUserService _userService;
    public UserController(ILogger<UserController> logger,IUserService userService)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("GetToken")]
    public async Task<IActionResult> GetToken(LoginModel loginModel){
          _logger.LogInformation("UserController:: GetToken -> Started");
          try{
            return  Ok(await _userService.GenerateToken(loginModel));
          }catch(Exception ex){
            return BadRequest(ex.Message);
          }
    }
}
