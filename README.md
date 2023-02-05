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

Send Email with SendGrid
1. https://www.courier.com/guides/csharp-send-email/

Send SMS with Twilio
2. https://app.sendgrid.com/twilio/sms

Key generation and data encryption 
3. https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.dataprotection.dataprotectionprovider.create?view=aspnetcore-7.0

Role-based authorization 
4. https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-7.0

Callback URL generation
5. https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.urlhelperextensions.page?view=aspnetcore-7.0

