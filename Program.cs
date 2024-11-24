using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Soufieb.Webapp.Config;
using System;
using Microsoft.EntityFrameworkCore;
using Soufieb.Webapp.Models;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services connect
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Contexto>(options =>
    options.UseSqlServer("Server=souFIEB.mssql.somee.com;Database=souFIEB;User Id=souFIEB_SQLLogin_1;Password=8agtvqatbv;Connect Timeout=20;Encrypt=False;TrustServerCertificate=False"));

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 6000000; // Define o tamanho máximo do upload de arquivos (exemplo de 6 MB).
});

// Add services to the container.
builder.Services.AddWebAppConfig(builder.Configuration, builder.WebHost, builder.Environment);

// Habilitar o uso da sessão
builder.Services.AddSession(options =>
{
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;

    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.UseWebAppConfig();

// Configuração de políticas de autorização
app.Use(async (context, next) =>
{
    // Se o usuário não estiver autenticado e tentar acessar uma página que requer autenticação
    if (!context.User.Identity.IsAuthenticated && !context.Request.Path.Equals("/login", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect("/Login");
        return;
    }

    // Se o usuário estiver autenticado e tentar acessar a página de login
    if (context.User.Identity.IsAuthenticated && context.Request.Path.Equals("/login", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect("/Home");
        return;
    }

    await next();
});


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Home}/{id?}");
});

app.MapControllers();

app.Run();
