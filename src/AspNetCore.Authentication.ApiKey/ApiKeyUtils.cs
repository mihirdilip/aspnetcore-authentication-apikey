// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AspNetCore.Authentication.ApiKey
{
	/// <summary>
	/// Utility class.
	/// </summary>
	internal static class ApiKeyUtils
	{
		/// <summary>
		/// Builds Claims Principal from the provided information. 
		/// If the <paramref name="claims"/> does not have claim of type <see cref="ClaimTypes.NameIdentifier"/> then <paramref name="ownerName"/> will be added as claim of type <see cref="ClaimTypes.NameIdentifier"/>.
		/// If the <paramref name="claims"/> does not have claim of type <see cref="ClaimTypes.Name"/> then <paramref name="ownerName"/> will be added as claim of type <see cref="ClaimTypes.Name"/>.
		/// </summary>
		/// <param name="ownerName">The owner name.</param>
		/// <param name="schemeName">The scheme name.</param>
		/// <param name="claimsIssuer">The claims issuer.</param>
		/// <param name="claims">The list of claims.</param>
		/// <returns></returns>
		internal static ClaimsPrincipal BuildClaimsPrincipal(string ownerName, string schemeName, string claimsIssuer, IEnumerable<Claim> claims = null)
		{
			if (string.IsNullOrWhiteSpace(schemeName)) throw new ArgumentNullException(nameof(schemeName));

			var claimsList = new List<Claim>();
			if (claims != null) claimsList.AddRange(claims);

			if (!string.IsNullOrWhiteSpace(ownerName))
			{
				if (!claimsList.Any(c => c.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase)))
				{
					claimsList.Add(new Claim(ClaimTypes.NameIdentifier, ownerName, ClaimValueTypes.String, claimsIssuer));
				}

				if (!claimsList.Any(c => c.Type.Equals(ClaimTypes.Name, StringComparison.OrdinalIgnoreCase)))
				{
					claimsList.Add(new Claim(ClaimTypes.Name, ownerName, ClaimValueTypes.String, claimsIssuer));
				}
			}

			return new ClaimsPrincipal(new ClaimsIdentity(claimsList, schemeName));
		}
	}
}
