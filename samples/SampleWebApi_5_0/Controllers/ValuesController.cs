using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;

namespace SampleWebApi_5_0.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		// GET api/values
		[HttpGet]
		public ActionResult<IEnumerable<string>> Get()
		{
			return new string[] { "value1", "value2" };
		}

		[HttpGet("claims")]
		public ActionResult<string> Claims()
		{
			var sb = new StringBuilder();
			foreach (var claim in User.Claims)
			{
				sb.AppendLine($"{claim.Type}: {claim.Value}");
			}
			return sb.ToString();
		}

		[HttpGet("forbid")]
		public new IActionResult Forbid()
		{
			return base.Forbid();
		}
	}
}
