using System.Formats.Asn1;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

    [HttpPost("token")]
    public async Task<IActionResult> GetToken(LoginModel loginModel){
          if(!ModelState.IsValid)
            return BadRequest();
          try{
            return  Ok(await _userService.GenerateToken(loginModel));
          }catch(Exception ex){
            return BadRequest(ex.Message);
          }
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(LoginModel model){
        if(!ModelState.IsValid){
          return BadRequest();
        }
        try{
          return Ok(await _userService.RegisterUser(model));
        }catch(Exception ex){
          return BadRequest(ex.Message);
        }
    }
    [HttpPost("addrole")]
    public  async Task<IActionResult> AddRole(RoleModel model){
      if(!ModelState.IsValid){
        return BadRequest();
      }
      try{
        return Ok(await _userService.AddRole(model));
      }catch(Exception ex){
        return BadRequest(ex.Message);
      }
    }
}
