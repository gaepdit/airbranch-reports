# Infrastructure integration tests

The tests in this project require a connection to a working database populated with data. The values in the tests were selected using data from the current `airbranch` dev database.

## Setup

Create a *testsettings.json* file with a valid connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=***",
  }
}
```
