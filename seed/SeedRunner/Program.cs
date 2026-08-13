using DotNetEnv;
using Neo4j.Driver;

// 1. Locate and load .env file from repo root
string? repoRoot = FindRepoRoot();
string? envPath = repoRoot != null ? Path.Combine(repoRoot, ".env") : null;

if (envPath != null && File.Exists(envPath))
{
    Env.Load(envPath);
}
else
{
    // Try fallback path two directories up from SeedRunner project/working directory
    string relativeEnv = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".env"));
    if (File.Exists(relativeEnv))
    {
        Env.Load(relativeEnv);
    }
}

// 2. Read environment variables
string? uri = Environment.GetEnvironmentVariable("COGNODB_URI");
string? username = Environment.GetEnvironmentVariable("COGNODB_USERNAME");
string? password = Environment.GetEnvironmentVariable("COGNODB_PASSWORD");

if (string.IsNullOrWhiteSpace(uri) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
{
    Console.Error.WriteLine("Error: Required environment variables are missing.");
    Console.Error.WriteLine("Please ensure COGNODB_URI, COGNODB_USERNAME, and COGNODB_PASSWORD are set in .env or your environment.");
    return 1;
}

// 3. Locate and read seed_data.cypher
string? seedFilePath = repoRoot != null ? Path.Combine(repoRoot, "seed", "seed_data.cypher") : null;
if (seedFilePath == null || !File.Exists(seedFilePath))
{
    string relativeSeed = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "seed_data.cypher"));
    if (File.Exists(relativeSeed))
    {
        seedFilePath = relativeSeed;
    }
    else if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "seed_data.cypher")))
    {
        seedFilePath = Path.Combine(Directory.GetCurrentDirectory(), "seed_data.cypher");
    }
    else
    {
        Console.Error.WriteLine("Error: seed_data.cypher file not found.");
        return 1;
    }
}

string rawScript = await File.ReadAllTextAsync(seedFilePath);

// Split into individual statements by semicolon and filter comments/empty lines
var rawChunks = rawScript.Split(';', StringSplitOptions.RemoveEmptyEntries);
var statements = new List<string>();

foreach (var chunk in rawChunks)
{
    var lines = chunk.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
    var cleanLines = lines
        .Where(line => !line.TrimStart().StartsWith("//"))
        .ToList();

    var statement = string.Join(Environment.NewLine, cleanLines).Trim();
    if (!string.IsNullOrWhiteSpace(statement))
    {
        statements.Add(statement);
    }
}

if (statements.Count == 0)
{
    Console.Error.WriteLine("Error: No valid Cypher statements found to execute.");
    return 1;
}

Console.WriteLine($"Found {statements.Count} Cypher statements to execute.");
Console.WriteLine($"Connecting to CognoDB at: {uri}");

// 4. Execute statements sequentially
try
{
    await using var driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password));
    await using var session = driver.AsyncSession();

    for (int i = 0; i < statements.Count; i++)
    {
        int statementNumber = i + 1;
        string statement = statements[i];

        Console.WriteLine($"[{statementNumber}/{statements.Count}] Executing statement...");

        try
        {
            var cursor = await session.RunAsync(statement);
            await cursor.ConsumeAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error executing statement [{statementNumber}/{statements.Count}]:");
            Console.Error.WriteLine(statement);
            Console.Error.WriteLine($"Exception: {ex.Message}");
            return 1;
        }
    }
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Database connection error: {ex.Message}");
    return 1;
}

Console.WriteLine($"Seed data applied successfully! {statements.Count} statements executed.");
return 0;

static string? FindRepoRoot()
{
    var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
    while (dir != null)
    {
        if (File.Exists(Path.Combine(dir.FullName, ".env")) ||
            File.Exists(Path.Combine(dir.FullName, ".env.example")) ||
            Directory.Exists(Path.Combine(dir.FullName, "seed")))
        {
            return dir.FullName;
        }
        dir = dir.Parent;
    }

    dir = new DirectoryInfo(AppContext.BaseDirectory);
    while (dir != null)
    {
        if (File.Exists(Path.Combine(dir.FullName, ".env")) ||
            File.Exists(Path.Combine(dir.FullName, ".env.example")) ||
            Directory.Exists(Path.Combine(dir.FullName, "seed")))
        {
            return dir.FullName;
        }
        dir = dir.Parent;
    }

    return null;
}
