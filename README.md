# JSON Web Tokens in .NET Core

JSON Web Tokens are an open, industry standard RFC 7519 method for representing claims securely between two parties.  Learn more [here](https://jwt.io/).

This project provides a means for generating and validating JSON web tokens in C#.  It uses .NET Core 3.1, along with the Microsoft.AspNetCore.Authentication.JwtBearer library.

The running service provides two endpoints:

* /Login (POST) - Generate a new token for a user.
* /Verify (GET) - Check the validity of a generated token.

Calls can be made to the running service by opening the service-calls.http file in Visual Studio Code.  It requires the REST Client extension.
