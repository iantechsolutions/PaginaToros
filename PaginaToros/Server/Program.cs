using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaginaToros.Server.Context;
using PaginaToros.Server.Utilidades;
using System.Text;
using PaginaToros.Server.Repositorio.Contrato;
using PaginaToros.Server.Repositorio.Implementacion;

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
/*AUTORIZACI�N*/
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

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("HerefordConnection"), ServerVersion.Parse("10.3.39-mariadb"), options => options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: System.TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)
                );
});
builder.Services.AddDbContext<hereford_prContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("HerefordConnection"), ServerVersion.Parse("10.3.39-mariadb"), options => options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: System.TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)
                );
});
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ICentrosiumRepositorio, CentrosiumRepositorio>();
builder.Services.AddScoped<ICertifsemanRepositorio, CertifsemanRepositorio>();
builder.Services.AddScoped<IDesepla1Repositorio, Desepla1Repositorio>();
builder.Services.AddScoped<IDesepla3Repositorio, Desepla3Repositorio>();
builder.Services.AddScoped<IEstableRepositorio, EstableRepositorio>();
builder.Services.AddScoped<IFutcontrolRepositorio, FutcontrolRepositorio>();
builder.Services.AddScoped<IInspectRepositorio, InspectRepositorio>();
builder.Services.AddScoped<IPlantelRepositorio, PlantelRepositorio>();
builder.Services.AddScoped<IResin1Repositorio, Resin1Repositorio>();
builder.Services.AddScoped<IResin2Repositorio, Resin2Repositorio>();
builder.Services.AddScoped<IResin3Repositorio, Resin3Repositorio>();
builder.Services.AddScoped<IResin4Repositorio, Resin4Repositorio>();
builder.Services.AddScoped<IResin6Repositorio, Resin6Repositorio>();
builder.Services.AddScoped<IResin8Repositorio, Resin8Repositorio>();
builder.Services.AddScoped<ISocioRepositorio, SocioRepositorio>();
builder.Services.AddScoped<ISolici1Repositorio, Solici1Repositorio>();
builder.Services.AddScoped<ISolici1AuxRepositorio, Solici1AuxRepositorio>();
builder.Services.AddScoped<ITorosRepositorio, TorosRepositorio>();
builder.Services.AddScoped<ITransanRepositorio, TransanRepositorio>();
builder.Services.AddScoped<ITranssbRepositorio, TranssbRepositorio>();
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
