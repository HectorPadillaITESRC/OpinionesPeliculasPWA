using Microsoft.EntityFrameworkCore;
using OpinionesPeliculasPWA.Models.Entities;
using OpinionesPeliculasPWA.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var cs = builder.Configuration.GetConnectionString("OpinionesConnectionString");
builder.Services.AddDbContext<OpinionespeliculasContext>(x =>
x.UseMySql(cs, MySqlServerVersion.AutoDetect(cs)));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


builder.Services.AddRazorPages();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OpinionespeliculasContext>();
    db.Database.Migrate();
}


app.MapControllers();
app.MapRazorPages();
app.UseStaticFiles();

app.Run();
