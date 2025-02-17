using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DependencyInjection;

[ApiController]
[Route("[controller]/[action]")]
public class TestController:ControllerBase
{
    private TestService _testService1;
    private TestService _testService2;
    public TestController(TestService testService1,TestService testService2)
    {
        _testService1 = testService1;
        _testService2 = testService2;
    }
    
    [HttpGet]
    public IActionResult GetGuid(){
        return Ok(
        new {
            Service1 = _testService1.GetGuid(),
            Service2 = _testService2.GetGuid(),
        }
        );
    }

}
