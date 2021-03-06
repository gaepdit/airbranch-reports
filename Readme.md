# Air Protection Branch Reports Printing Service

This application creates various printable reports used or provided by the Georgia EPD Air Protection Branch.

[![.NET Test](https://github.com/gaepdit/airbranch-reports/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gaepdit/airbranch-reports/actions/workflows/dotnet.yml) 
[![CodeQL](https://github.com/gaepdit/airbranch-reports/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/gaepdit/airbranch-reports/actions/workflows/codeql-analysis.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_airbranch-reports&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=gaepdit_airbranch-reports)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_airbranch-reports&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=gaepdit_airbranch-reports)

## Info for Developers

The Air Reports application is written using .NET 6.

### Project organization

The solution contains four projects:

* Domain - A class library containing the data models and business logic.
* LocalRepository - A class library implementing the domain repositories using sample data for use in local development.
* Infrastructure - A class library implementing the domain repository using a SQL Server database.
* WebApp - The front end web application.

There are also corresponding unit test projects.

### Launch profiles

Run locally by choosing the "Local" launch profile or connect to the dev database by choosing the "Dev Server" profiles. 

#### Local launch profile

 In this configuration, no database is used -- sample data is provided in the "LocalRepository/Data" folder. 
 
 No authentication provider is enabled, either. The `AuthenticatedUser` setting in the "appsettings.Local.json" file can be used to set whether the application runs with an authenticated user or not.

 ```json
{
  "AuthenticatedUser": true
}
```

 #### Dev Server launch profile

 To use the Dev Server configuration, copy the file named "appsettings.Development.json" from the "app-config" repo into the "src/WebApp" folder. In this configuration, the existing SQL Server `airbranch` database will be used.

### Testing

The Domain, LocalRepository, and WebApp test projects all run using local data. To run the Infrastructure integration tests, copy the file named "testsettings.json" from the "app-config" repo into the "tests/IntegrationTests" folder.
