| Attribute	 | Details |
| ------------- | ----------- |
| ORM  | EF Core  |
| DataBase  | SQL Server  |
| Architecture  | Clean  |
|Auth system |JWT|
<br/>
<br/>
<br/>
This repository contains a simple .netcore WEB API application with three endpoints to "register" and "login" users and giving them JWT token to use another authorized Action.
<br/>
<br/>
If their token's expiration time reaches , they can use "refresh token" and gain another jwt token.
<br/>
<br/>
And if their refresh token's expiration time reaches , they can login again and get another JWT token and refresh token.
<br/>
<br/>
