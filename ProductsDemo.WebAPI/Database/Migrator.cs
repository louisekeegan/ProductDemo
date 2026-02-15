using System.Reflection;
using DbUp;
using Npgsql;
using Polly;
using Polly.Retry;

namespace ProductsDemo.WebAPI.Database;

public static class Migrator
{
    public static void RunMigrations(string connectionString)
    {
        // Define a retry policy: retry up to 15 times, 1 second apart, on any exception
        RetryPolicy retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount: 15,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(1),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"Waiting for database... attempt {retryCount}/15");
                });

        // Execute the connection check under the retry policy
        retryPolicy.Execute(() =>
        {
            using var testConn = new NpgsqlConnection(connectionString);
            testConn.Open(); // throws if DB is not ready
        });

        // Ensure database exists
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        // Run DbUp migrations
        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
            throw result.Error;
        }
    }
}