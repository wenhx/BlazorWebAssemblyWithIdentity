using BlazorWebAssemblyWithIdentity.Server;
using BlazorWebAssemblyWithIdentity.Server.Data;
using BlazorWebAssemblyWithIdentity.Server.Models;
using BlazorWebAssemblyWithIdentity.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlazorWebAssemblyWithIdentity;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Filename=Application.db"));
        builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        builder.Services.Configure<IdentityOptions>(options => 
        { 
            options.Password.RequireDigit = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = Constants.Models.MinimumPasswordLength;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 3;

            options.User.RequireUniqueEmail = false;
        });
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            };
        });

        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options => 
            {
                options.InvalidModelStateResponseFactory = context => 
                {
                    var error = context.ModelState.Where(state => state.Value != null)
                                                    .SelectMany(state => state.Value!.Errors)
                                                    .Select(error => error.ErrorMessage)
                                                    .First();
                    return new BadRequestObjectResult(InvokedResult.Fail(error));
                };
            });
        builder.Services.AddRazorPages();
        var tokenKey = builder.Configuration.GetValue<string>("TokenKey");
        if (String.IsNullOrEmpty(tokenKey))
            throw new InvalidOperationException("The token wasn't found in the appsetting.json file.");
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        builder.Services.AddSingleton(sp => 
        {
            return new AppSettings
            { 
                TokenKey = tokenKey
            };
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //Note: Microsoft recommends to NOT migrate your database at Startup. 
                //You should consider your migration strategy according to the guidelines.
                serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
            }

            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}