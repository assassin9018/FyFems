global using DAL.Models;
global using DAL.Repository;
global using MyFems.Dto;
global using MyFemsApi.Exceptions;
using DAL.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyFemsApi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    string connection = builder.Configuration.GetConnectionString("DefaultConnection");
    services.AddNpgsql<MyFemsDbContext>(connection);
    services.AddScoped<UnitOfWork>();

    services.AddAutoMapper(Assembly.GetAssembly(typeof(MapperProfile)));
    services.AddScoped<PasswordHasher<User>>();
    services.Configure<PasswordHasherOptions>(option =>
    {
        option.IterationCount = 1000;
    });
    services.Configure<IdentityOptions>(options =>
    {
        // Default Password settings.
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    AddJwt(services);
    //services.AddAuthentication("Coockies").AddCookie();
}

void AddJwt(IServiceCollection services)
{
    ConfigOptions.JwtAuthOptions jstOptions = new(builder.Configuration);
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // указывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    // строка, представляющая издателя
                    ValidIssuer = jstOptions.Issuer,
                    // будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    // установка потребителя токена
                    ValidAudience = jstOptions.Audience,
                    // будет ли валидироваться время существования
                    ValidateLifetime = true,
                    // установка ключа безопасности
                    IssuerSigningKey = jstOptions.GetSymmetricKey(),
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,
                };
            });
}