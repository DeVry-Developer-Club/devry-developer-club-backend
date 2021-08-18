Authentication
-----
[Back](../../Readme.md)

Pretty much every application these days require some sort of user tracking mechanism. 
We currently have a very basic implementation of`Github` OAuth. Meaning you can sign in via Github!

### Tokens / Keys / Secrets
As mentioned in [Database.md](Database.md) - we utilize user secrets to prevent sensitive information from leaking. 

### Github
Until a streamlined process has been made -- reach out to a senior contributor to obtain
the client secret and client id. Then run the following commands from within the `DevryDeveloperClub` project folder

```bash
dotnet user-secrets set "Github:ClientId" "insert id here"
dotnet user-secrets set "Github:ClientSecret" "insert secret here"
```

[Back](../../Readme.md)