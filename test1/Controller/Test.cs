using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace test1;

[ApiController]
[Route("controller")]
[Authorize]
public class Test : ControllerBase
{
		[HttpGet("TestAPI")]
		public async Task<IActionResult> Testing()
		{
				return Ok("Testing passed!");
		}

}
