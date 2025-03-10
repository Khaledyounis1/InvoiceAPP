
using InvoiceApp.Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using Serilog;

using InvoiceApp.Api.Services.InvoiceService;
using InvoiceApp.Api.Services.StoreService;
using InvoiceApp.Infrastructure.GenericRepositories;
using InvoiceAppData.Models;
using InvoiceApp.Infrastructure.Abstracts;
using InvoiceApp.Infrastructure;


namespace InvoiceApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<Appdbcontext>(
                Options => 
                    {
                        Options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
                    }
                );
            builder.Services.AddInfrastructureDependencies();
            //builder.Services.AddScoped<IinvoiceRepository, invoiceRepository>();
            builder.Services.AddScoped<IinvoiceService, InvoiceService>();
            //builder.Services.AddScoped<IStoreRepository, StoreRepository>();
            builder.Services.AddScoped<IStoreService, StoreService>();
           // builder.Services.AddScoped<IinvoiceitemService, invoiceitemService>();
           // builder.Services.AddScoped<IinvoiceitemRepository, InvoiceitemRepository>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<Appdbcontext>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            // builder.Services.AddSwaggerGen();
            #region Edit SWagger
            builder.Services.AddSwaggerGen(c =>
            {
                // Adding the authorization header to Swagger UI
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Description = "Enter your JWT token in the format **'Bearer {your_token}'**"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });


            #endregion


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });


            //JWT
            builder.Services.AddAuthentication(options => {

                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options => {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:IssuerIP"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:AudienceIP"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.
                    UTF8.GetBytes(builder.Configuration["JWT:Secritkey"]))
                };

            });
        //Serilog 
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();
            
            builder.Services.AddSerilog();




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");
            app.UseAuthorization();


            app.MapControllers();

            app.Run();

            Log.CloseAndFlush();
        }
    }
}
