Authentication
-----
[Back](../../Readme.md)

Pretty much every application these days require some sort of user tracking mechanism. 
We currently have a very basic implementation of`Github` OAuth. Meaning you can sign in via Github!

### Tokens / Keys / Secrets
As mentioned in [Database.md](Database.md) - we utilize user secrets to prevent sensitive information from leaking. 

In our application JWT tokens contain the claims (aka information about the user). This gets encrypted, and the result is
passed down to the user during authentication.

#### How you can get a token for authentication purposes
```bash
curl -X POST "https://localhost:5001/api/Auth/login" 
  -H  "accept: */*" 
  -H  "Content-Type: application/json" 
  -d "{\"username\":\"string\",\"password\":\"string\"}"
```


#### How to utilize the token you acquired above
```bash
curl -X POST "https://localhost:5001/api/Authenticate/auth" 
    -H  "accept: */*" 
    -H  "Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiVGVzdFVzZXIiLCJqdGkiOiIyODZkNmM0Ny1iZTJlLTQzYjAtYTJlYy1jNjU0NTdlMTRkZmEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTYyOTUxNzIyMSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEifQ.jnbNKK04bbNbHgcfcqQM6CipqeVS3s4AHa5u_q64smc"
```

### Github
Until a streamlined process has been made -- reach out to a senior contributor to obtain
the client secret and client id. Then run the following commands from within the `DevryDeveloperClub` project folder

```bash
dotnet user-secrets set "Github:ClientId" "insert id here"
dotnet user-secrets set "Github:ClientSecret" "insert secret here"
```

[Back](../../Readme.md)