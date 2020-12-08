using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;

namespace SampleWebApi_2_2.Controllers
{
	[Route("api/[controller]")]
	public class ValuesController : ControllerBase
	{
		// GET api/values
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		[HttpGet("claims")]
		public string Claims()
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
