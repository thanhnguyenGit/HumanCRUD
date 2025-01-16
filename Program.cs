using Microsoft.EntityFrameworkCore;
using BSS;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddControllers().AddNewtonsoftJson();
//Database Connection
BSS.DBM.ConnectionString = "Data Source=LAPTOP-TGNNOARE\\SQLEXPRESS;Initial Catalog=ApiStudy1;Integrated Security=True;Encrypt=False";

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Human/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller=Human}/{action=GetAll}/{id?}");

app.Run();
