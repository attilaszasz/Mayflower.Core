using Mayflower;
using Microsoft.Extensions.Configuration;
using System;

namespace MayflowerCLI.Core
{
    class Program
    {
        static void Main()
        {
            var config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .AddJsonFile("connectionstrings.json")
                            .Build();

            var options = new Options
            {
                ConnectionStringName = config["connectionstring"],
                CommandTimeout = 30,
                Force = config["force"].Equals("true", StringComparison.InvariantCultureIgnoreCase),
                MigrationsFolder = config["folder"],
                MigrationsTable = config["table"],
                UseGlobalTransaction = config["global"].Equals("true", StringComparison.InvariantCultureIgnoreCase),
                IsPreview = config["preview"].Equals("true", StringComparison.InvariantCultureIgnoreCase),
                Output = Console.Out
            };

            options.ConnectionString = config[options.ConnectionStringName];

            var result = Migrator.RunOutstandingMigrations(options);
            if (!result.Success)
                Environment.Exit(1);
            Environment.Exit(0);
        }
    }
}
