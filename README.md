# Mayflower.Core

Mayflower is a simple, forward-only, database migrator for SQL Server based on the migrator which Stack Overflow uses.

This is a port to .Net Core of Bret Cope's [Mayflower.NET](https://github.com/bretcope/Mayflower.NET) project.  

## Usage

### Creating Migrations

A migration is just plain T-SQL saved in a .sql file. Individual commands are separated with the `GO` keyword, just like when using [SSMS](https://msdn.microsoft.com/en-us/library/mt238290.aspx). For example:

```sql
CREATE TABLE One
(
  Id int not null identity(1,1),
  Name nvarchar(50) not null,
  
  constraint PK_One primary key clustered (Id)
)
GO

INSERT INTO One (Name) VALUES ('Wystan')
GO
```

> Migrations are run in a transaction by default, which allows them to be rolled back if any command fails. You can disable this transaction for a specific migration by beginning the file with `-- no transaction --`.

We recommend prefixing migration file names with a zero-padded number so that the migrations are listed in chronological order. For example, a directory of migrations might look like:

```
0001 - Add Users table.sql
0002 - Add Posts.sql
0003 - Insert default users.sql
0004 - Add auth columns to Users.sql
...
```

### Running Migrations

#### Command Line

The easiest way to run migrations is from command line. It requires .NET Core 2 or above.

Typical usage is simply:

```
dotnet MayflowerCLI.Core.dll
```

Various options can be changed in the [appsettings.json](https://github.com/attilaszasz/Mayflower.Core/blob/master/MayflowerCLI.Core/appsettings.json) file. The connection string to the database is in the [connectionstrings.json](https://github.com/attilaszasz/Mayflower.Core/blob/master/MayflowerCLI.Core/connectionstrings.json) file:

```
appsettings.json

  "connectionstring"         The path to the connection string to be used from the connectionstrings.json.
  "folder"                   The folder containing your .sql migration files
                               (defaults to current working directory).
  "timeout"                  Command timeout duration in seconds (default: 30)
  "preview"                  Run outstanding migrations, but roll them back.
  "global"                   Run all outstanding migrations in a single
                               transaction, if possible.
  "table"                    Name of the table used to track migrations
                               (default: Migrations)
  "force"                    Will rerun modified migrations.
```

```
connectionstrings.json

{
  "ConnectionStrings": {
    "Data": "YourConnectionString"
  }
}
```
#### Programmatic

If you'd prefer, Mayflower can be called via code. Include Mayflower.Core in your solution and reference it in your project.

```csharp
var options = new Options
{
    ConnectionString = "YourConnectionString",
    MigrationsFolder = @"c:\path\to\migrations",
    Output = Console.Out,
};

var result = Migrator.RunOutstandingMigrations(options);
// result.Success indicates success or failure
```

The `Options` class has equivalent properties to most of the command line options.

### Reverting Migrations

Many migration systems have a notion of reversing a migration or "downgrading" in some sense. Mayflower has no such concept. If you want to reverse the effects of one migration, then you write a new migration to do so. Mayflower lives in a forward-only world.

## License

Mayflower is available under the [MIT License](https://github.com/attilaszasz/Mayflower.Core/blob/master/LICENSE).
