﻿using System.Security.Claims;

namespace Djm.OGame.Web.Api.Services.Authentication
{
    public interface IJwtFactory
    {
        string GenerateToken(Claim[] claims);
        Claim[] GenerateClaims(int id, string role);
    }
}