# Air Protection Branch Reports Printing Service

This application creates various printable reports used or provided by the Georgia EPD Air Protection Branch.

[![.NET Test](https://github.com/gaepdit/airbranch-reports/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gaepdit/airbranch-reports/actions/workflows/dotnet.yml) 
[![CodeQL](https://github.com/gaepdit/airbranch-reports/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/gaepdit/airbranch-reports/actions/workflows/codeql-analysis.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_airbranch-reports&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=gaepdit_airbranch-reports)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_airbranch-reports&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=gaepdit_airbranch-reports)

## Project organization

The solution contains four projects:

* Domain - A class library containing the data models and business logic.
* LocalRepository - A class library implementing the domain repositories using sample data for use in local development.
* Infrastructure - A class library implementing the domain repository using a SQL Server database.
* WebApp - The front end web application.

There are also corresponding unit test projects.

## Development

Run locally by choosing one of the "Local" launch profiles or connect to a database by choosing one of the "Dev Server" profiles. 

### Local 

 In this configuration, no database is used -- sample data is provided in the `LocalRepository/Data` folder. 
 
 No authentication provider is enabled, either. The `AuthenticatedUser` setting in the `appsettings.Local.json` file can be used to set whether the application runs with an authenticated user or not.

 ```json
{
  "AuthenticatedUser": true
}
```

 ### Dev Server

 To use the Dev Server configuration, add a file named `appsettings.Development.json` and provide a database connection string and Azure AD configuration settings.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "qualified.domain.name",
    "TenantId": "00000000-0000-0000-0000-000000000000",
    "ClientId": "00000000-0000-0000-00000000000000000",
    "CallbackPath": "/signin-oidc"
  }
}
```

## Testing

The Domain, LocalRepository, and WebApp test projects all run using local data. To run the Integration tests, add a file named `IntegrationTests/testsettings.json` and provide a database connection string.
