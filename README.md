# Fresh Farm Market

1. Make sure to have .NET Core SDK installed

```
Download link: https://www.microsoft.com/net/download/core
```

2. Installed Required Packages

```
dotnet add package Microsoft.EntityFrameworkCore -v 6.0.12
dotnet add package Microsoft.EntityFrameworkCore.Tools -v 6.0.12
dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 6.0.12
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore -v 6.0.12
```

# To run the application, follow steps below

## Step 1

Run the following command to install dotnet-ef command

```
dotnet tool install --global dotnet-ef
```

## Step 2

Run the following command to add migration for creation of database

```
dotnet ef migrations add InitialCreate
```

## Step 3

Run the following command to create database

```
dotnet ef database update
```

# References

1. https://stackoverflow.com/questions/70728537/why-do-i-get-warning-cs8602
2. https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/suppress-warnings
3. https://codeasy.net/lesson/input_validation
4. https://stackoverflow.com/questions/7253601/c-converting-string-variable-to-class-variable
5. https://www.tutorialsteacher.com/articles/generate-random-numbers-in-csharp
6. https://stackoverflow.com/questions/852181/c-printing-all-properties-of-an-object
7. https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.typedescriptor?view=net-6.0
