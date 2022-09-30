﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JWT.Models; // класс Person
using JWT;

namespace JWT.Controllers
{
    public class AccountController : Controller
    {

        private ApplicationContext db;

        public AccountController(ApplicationContext context)
        {
            db = context;
        }

        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
issuer: AuthOptions.ISSUER,
audience: AuthOptions.AUDIENCE,
notBefore: now,
claims: identity.Claims,
expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }

        [HttpGet("/test")]
        public IActionResult Test()
        {
            return Ok();
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            Person person = db.Persons.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (person != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                    };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            // если пользователя не найдено
            return null;
        }
    }
}