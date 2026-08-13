using EquityGraph.Api.Features.Companies.GetBoardInterlocks;
using EquityGraph.Api.Features.Companies.GetCompanyDetail;
using EquityGraph.Api.Features.Companies.GetInstitutionalOverlap;
using EquityGraph.Api.Features.Companies.GetShortestPath;
using EquityGraph.Api.Features.Companies.GetSupplyChainExposure;
using EquityGraph.Api.Features.Companies.ListCompanies;
using EquityGraph.Api.Features.Health.CheckDbHealth;
using EquityGraph.Api.Shared.CognoDb;

// Load environment variables from .env file
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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
