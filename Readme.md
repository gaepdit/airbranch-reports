# Air Protection Branch Reports Printing Service

This application creates various printable reports used or provided by the Georgia EPD Air Protection Branch.

[![Georgia EPD-IT](https://raw.githubusercontent.com/gaepdit/gaepd-brand/main/blinkies/blinkies.cafe-gaepdit.gif)](https://github.com/gaepdit)
[![.NET Test](https://github.com/gaepdit/airbranch-reports/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gaepdit/airbranch-reports/actions/workflows/dotnet.yml) 
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_airbranch-reports&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=gaepdit_airbranch-reports)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_airbranch-reports&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=gaepdit_airbranch-reports)

## Info for Developers

The Air Reports application is written using .NET 7.

### Project organization

The solution contains four projects:

* Domain - A class library containing the data models and business logic.
* LocalRepository - A class library implementing the domain repositories using sample data for use in local development.
* Infrastructure - A class library implementing the domain repository using a SQL Server database.
* WebApp - The front end web application.

There are also corresponding unit test projects.

### Dev Settings

Configure the app when running locally by adding a `DevOptions` section to an app settings file:

```json
{
  "DevOptions": {
    "UseLocalData": true,
    "UseLocalAuth": true,
    "LocalAuthSucceeds": false
  }
}
```

* `UseLocalData`: When `true`, no database is used. Instead, sample data is used from the "LocalRepository/Data" folder. When `false`, a database connection string is used to connect to a database.
* `UseLocalAuth`: When `true`, no external authentication provider is enabled. When `false`, Azure AD is used for authentication.
  * `LocalAuthSucceeds`: Only used with `UseLocalAuth = true`. When `true`, local authentication is successful. When `false`, local authentication fails.

### Testing

The Domain, LocalRepository, and WebApp test projects all run using local data. To run the Infrastructure integration tests, copy the file named "testsettings.json" from the "app-config" repo into the "tests/IntegrationTests" folder.
