﻿using ApiCatalogo.Models;
using ApiCatalogo.Services;
using Microsoft.AspNetCore.Authorization;

namespace ApiCatalogo.ApiEndPoints
{
    public static class AutenticacaoEndpoints
    {
        public static void MapAutenticaoEndpoints(this WebApplication app)
        {
            app.MapPost("/login", [AllowAnonymous] async (UserModel userModel, ITokenService tokenService) =>
            {
                if (userModel == null)
                {
                    return Results.BadRequest("Login Inválido");
                }
                if (userModel.UserName == "macoratti" && userModel.Password == "numsey#123")
                {
                    var tokenString = tokenService.GerarToken(
                        app.Configuration["Jwt:Key"],
                        app.Configuration["Jwt:Issuer"],
                        app.Configuration["Jwt:Audience"],
                        userModel
                    );

                    return Results.Ok(new { token = tokenString });
                }
                else
                {
                    return Results.BadRequest("Login Inválido");
                }
            }).Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .WithName("Login")
            .WithTags("Autenticacao");
        }
    }
}