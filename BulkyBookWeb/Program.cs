
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
// create a web application builder object
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer
//    (builder.Configuration.GetConnectionString("DefaultConnection"),
//        option => option.MigrationsAssembly("BulkyBook.DataAccess")
//    )
//);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

var mvcBuilder = builder.Services.AddRazorPages();

//if (builder.Environment.IsDevelopment())
//{
//    mvcBuilder.AddRazorRuntimeCompilation();
//}


var app = builder.Build();

// Configure the HTTP request pipeline - specifies how application should respond to a web request.
/// check if this development is not in the environment
///      then, redirect to another page
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


app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Home}/{action=Index}/{id?}"
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"
    );

app.Run();
