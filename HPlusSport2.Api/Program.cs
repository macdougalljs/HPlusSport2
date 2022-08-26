using HPlusSport2.Api.Models;
using HPlusSport2.Api.Classes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        //   options.SuppressModelStateInvalidFilter = true
    });
;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ShopContext>(options =>
{
    options.UseInMemoryDatabase("Shop");
});  // needed to be added

builder.Services.AddApiVersioning(options =>   // needed to be added
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;

    options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
});

/*
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
    builder.WithOrigins("https://localhost:7176")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
*/

// builder.Services.AddVersionedApiExplorer(
//                options => options.GroupNameFormat = "'v'VVV");


var app = builder.Build();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

// app.UseCors();  // the location of this is important,  needs to be after Redirection and before authorization

app.UseAuthorization();

app.MapControllers();

app.Run();
