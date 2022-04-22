using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;

namespace SampleWebApi_2_2.Controllers
{
	using Microsoft.AspNetCore.Authorization;

	[Route("api/[controller]")]
	public class ValuesController : ControllerBase
	{
		// GET api/values
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/values/ones
		[HttpGet("ones")]
		[Authorize(AuthenticationSchemes = "Test2")]
		public IEnumerable<string> Get_Test2()
		{
			return new string[] { "value1" };
		}

		// GET api/values/twos
		[HttpGet("twos")]
		[Authorize(AuthenticationSchemes = "Test3")]
		public IEnumerable<string> Get_Test3()
		{
			return new string[] { "value2" };
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
