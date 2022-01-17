global using DAL.Repository;
global using MyFemsApi.Exceptions;
using DAL.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyFemsApi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    string connection = builder.Configuration.GetConnectionString("DefaultConnection");
    services.AddNpgsql<MyFemsDbContext>(connection);
    services.AddScoped<UnitOfWork>();

    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

AddJwt(services);
services.AddAuthentication("Coockies").AddCookie();
}

void AddJwt(IServiceCollection services)
{
    ConfigOptions.JwtAuthOptions jstOptions = new(builder.Configuration);
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // ���������, ����� �� �������������� �������� ��� ��������� ������
                    ValidateIssuer = true,
                    // ������, �������������� ��������
                    ValidIssuer = jstOptions.Issuer,
                    // ����� �� �������������� ����������� ������
                    ValidateAudience = true,
                    // ��������� ����������� ������
                    ValidAudience = jstOptions.Audience,
                    // ����� �� �������������� ����� �������������
                    ValidateLifetime = true,
                    // ��������� ����� ������������
                    IssuerSigningKey = jstOptions.GetSymmetricKey(),
                    // ��������� ����� ������������
                    ValidateIssuerSigningKey = true,
                };
            });
}