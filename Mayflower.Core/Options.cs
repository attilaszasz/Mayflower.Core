using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Mayflower
{
    public class Options
    {
        public string ConnectionString { get; set; }
        public string ConnectionStringName { get; set; }
        public int CommandTimeout { get; set; } = 30;
        public string MigrationsFolder { get; set; }
        public bool IsPreview { get; set; }
        public bool UseGlobalTransaction { get; set; }
        public string MigrationsTable { get; set; }
        public TextWriter Output { get; set; }
        public bool Force { get; set; }
        public DatabaseProvider Provider { get; set; }

        public void AssertValid()
        {
            if (string.IsNullOrWhiteSpace(ConnectionStringName))
            {
                throw new Exception("A connection string name must be specified.");
            }

            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                throw new Exception("The connection string name must exist in the connectionstrings.json file.");
            }

            if (!string.IsNullOrEmpty(MigrationsTable))
            {
                if (!Regex.IsMatch(MigrationsTable, "^[a-zA-Z]+$"))
                    throw new Exception("Migrations table name can only contain letters A-Z.");
            }
        }

        internal string GetMigrationsTable()
        {
            return string.IsNullOrEmpty(MigrationsTable) ? "Migrations" : MigrationsTable;
        }

        internal string GetFolder()
        {
            return string.IsNullOrEmpty(MigrationsFolder) ? Directory.GetCurrentDirectory() : MigrationsFolder;
        }
    }
}