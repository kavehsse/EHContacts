# EHContacts
Projects:
<p>
&emsp;EHContacts - ASP.NET Core 2.1 - Entity Framework Core for data storage - Swagger UI for API Documentation/Test
<br>
&emsp;EHContacts.IntegrationTests - Test Contacts REST API

Project is hosted on Azure at https://ehcontacts.azurewebsites.net/swagger/index.html

Contacts REST API requires authorization using a bearer token.

Create bearer token using /api/v1/Accounts/CreateToken in the Accounts Controller with POST data

{
  "username": "hyoung@evh.com",
  "password": "P@ssw0rd!"
}

Copy the token in response, and click on swagger Authorize button. Enter Bearer <token_value>, click on Authorize. 
            
