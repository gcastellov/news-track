﻿using System;
using System.Linq;
using System.Security;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace NewsTrack.WebApi.Components
{
    internal class IdentityHelper : IIdentityHelper
    {
        private readonly ClaimsPrincipal _identity;

        public Guid Id => Guid.Parse(GetClaim(ClaimTypes.NameIdentifier).Value);
        public string Username => GetClaim(ClaimTypes.Name).Value;

        public IdentityHelper(IHttpContextAccessor accessor)
        {
            _identity = accessor.HttpContext.User;
        }

        private Claim GetClaim(string id)
        {
            if (!_identity.Identity.IsAuthenticated)
            {
                throw new SecurityException("Not authenticated");
            }

            return _identity.Claims.FirstOrDefault(c => c.Type == id);
        }
    }
}