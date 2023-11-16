using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Client.Servicios.Implementacion;
using PaginaToros.Server.Context;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var MyCors = "_MyCors";
IConfiguration Configuration = builder.Configuration;

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyCors,
        builder =>
        {
            builder.WithOrigins("*");
        }
        );
});
/*AUTORIZACIÓN*/
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
        ClockSkew = System.TimeSpan.Zero
    });
/*FIN*/
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql("server=vxsct3514.avnam.net;port=3306;user=herefordapp_com_ar;password=RWEr4dod6g3G;persist security info=True;database=hereford_pr;convert zero datetime=True", ServerVersion.Parse("10.3.39-mariadb")));
builder.Services.AddScoped<ITorosServicio, TorosServicio>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.UseCors(MyCors);

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
