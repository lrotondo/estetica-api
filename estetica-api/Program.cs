using dentist_panel_api;
using dentist_panel_api.Entities;
using dentist_panel_api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
//builder.WebHost.UseKestrel(o => o.ListenLocalhost(Int32.Parse(port)));

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<HealthCoverageServices>();
builder.Services.AddScoped<GenericServices>();
builder.Services.AddScoped<PatientsServices>();
builder.Services.AddScoped<AppointmentsServices>();
builder.Services.AddScoped<EmailServices>();
builder.Services.AddScoped<DoctorsServices>();
builder.Services.AddScoped<ConsultsServices>();
builder.Services.AddScoped<AWSBucketServices>();
builder.Services.AddScoped<MessagesServices>();
builder.Services.AddScoped<TipoDeTratamientoServices>();


string connectionString = Environment.GetEnvironmentVariable("ESTETICA_DB_CONNECTION") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContextPool<ApplicationDbContext>(options => options
    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), x => x.EnableRetryOnFailure()
        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors(), poolSize: 128);

string jwtKey = Environment.GetEnvironmentVariable("ESTETICA_JWT_KEY");
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ClockSkew = TimeSpan.Zero
            });

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User = new UserOptions
    {
        RequireUniqueEmail = true
    };
}).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
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
                        new string[]{}
                    }
                });

    var fileXML = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var routeXML = Path.Combine(AppContext.BaseDirectory, fileXML);
    c.IncludeXmlComments(routeXML, includeControllerXmlComments: true);
});

builder.Services.AddCors(c => c.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.WebHost.UseUrls("http://*:" + port);

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dentist Panel API");
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
