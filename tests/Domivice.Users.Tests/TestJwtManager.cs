using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Domivice.Users.Tests;

public static class TestJwtManager
{
    private static readonly JwtSecurityTokenHandler STokenHandler = new();
    private static readonly RandomNumberGenerator SGenerator = RandomNumberGenerator.Create();
    private static readonly byte[] SKey = new byte[32];

    static TestJwtManager()
    {
        SGenerator.GetBytes(SKey);
        SecurityKey = new SymmetricSecurityKey(SKey) { KeyId = Guid.NewGuid().ToString() };
        SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
    }
    public static string Issuer { get; } = Guid.NewGuid().ToString();
    public static string Audience { get; } = Guid.NewGuid().ToString();
    public static SecurityKey SecurityKey { get; }
    private static SigningCredentials SigningCredentials { get; }

    public static string GenerateJwtToken(IEnumerable<Claim>? claims = null)
    {
        return STokenHandler.WriteToken(new JwtSecurityToken(Issuer, Audience, claims, null,
            DateTime.UtcNow.AddMinutes(10), SigningCredentials));
    }
}