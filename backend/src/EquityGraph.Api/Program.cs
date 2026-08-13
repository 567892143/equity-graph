using EquityGraph.Api.Features.Companies.GetBoardInterlocks;
using EquityGraph.Api.Features.Companies.GetCompanyDetail;
using EquityGraph.Api.Features.Companies.GetInstitutionalOverlap;
using EquityGraph.Api.Features.Companies.GetShortestPath;
using EquityGraph.Api.Features.Companies.GetSupplyChainExposure;
using EquityGraph.Api.Features.Companies.ListCompanies;
using EquityGraph.Api.Features.Health.CheckDbHealth;
using EquityGraph.Api.Shared.CognoDb;
using EquityGraph.Api.Shared.Middleware;
using Microsoft.OpenApi.Models;

// Load environment variables from .env file
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Add Swagger/OpenAPI with API metadata
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EquityGraph API",
        Description = "Explore hidden relationship risk between companies via a graph database",
        Version = "v1"
    });
});

// Configure CognoDb options with environment variable overrides
builder.Services.Configure<CognoDbOptions>(options =>
{
    builder.Configuration.GetSection(CognoDbOptions.SectionName).Bind(options);

    var envUri = Environment.GetEnvironmentVariable("COGNODB_URI");
    if (!string.IsNullOrWhiteSpace(envUri))
    {
        options.Uri = envUri;
    }

    var envUsername = Environment.GetEnvironmentVariable("COGNODB_USERNAME");
    if (!string.IsNullOrWhiteSpace(envUsername))
    {
        options.Username = envUsername;
    }

    var envPassword = Environment.GetEnvironmentVariable("COGNODB_PASSWORD");
    if (!string.IsNullOrWhiteSpace(envPassword))
    {
        options.Password = envPassword;
    }
});

// Register CognoDB driver connection factory (singleton manages internal connection pool)
builder.Services.AddSingleton<CognoDbConnectionFactory>();

// Register CypherReader (singleton: stateless and thread-safe)
builder.Services.AddSingleton<ICypherReader, CypherReader>();

// Register Feature Query Handlers (Scoped)
builder.Services.AddScoped<ListCompaniesQueryHandler>();
builder.Services.AddScoped<GetCompanyDetailQueryHandler>();
builder.Services.AddScoped<GetBoardInterlocksQueryHandler>();
builder.Services.AddScoped<GetInstitutionalOverlapQueryHandler>();
builder.Services.AddScoped<GetSupplyChainExposureQueryHandler>();
builder.Services.AddScoped<GetShortestPathQueryHandler>();

// Configure CORS
var frontendOrigin = Environment.GetEnvironmentVariable("FRONTEND_ORIGIN");
if (string.IsNullOrWhiteSpace(frontendOrigin))
{
    frontendOrigin = "http://localhost:4200";
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(frontendOrigin)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Global Exception Handling Middleware (must wrap the pipeline early)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "EquityGraph API v1");
    });
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowFrontend");

// Map Health Endpoints
app.MapCheckDbHealthEndpoint();

// Map Company Feature Endpoints
app.MapListCompaniesEndpoint();
app.MapGetCompanyDetailEndpoint();
app.MapGetBoardInterlocksEndpoint();
app.MapGetInstitutionalOverlapEndpoint();
app.MapGetSupplyChainExposureEndpoint();
app.MapGetShortestPathEndpoint();

app.Run();
