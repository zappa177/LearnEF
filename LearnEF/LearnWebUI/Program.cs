using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
    //configuration from appsettings.json
    .ReadFrom.Configuration(context.Configuration)
    //configuration from services
    .ReadFrom.Services(services);
});





//
builder.Services.AddControllersWithViews();
//add services
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonService, PersonService>();

//add context 
builder.Services.AddDbContext<PersonsDbContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
// Đăng ký HttpLogging
builder.Services.AddHttpLogging(options =>
{
    // Cấu hình mức độ log
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
});



var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}
Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

//usehttplogging
app.UseHttpLogging();





app.UseStaticFiles();
app.UseRouting();
app.MapControllers();




app.Run();
