# Database
[Back](../../Readme.md)

Since we leverage `EntityFrameworkCore` we can technically shift around to any `DbProvider` without modifying our code. Minus where we say
`UseXYZ` in our `Infrastructure` project. 

Development
----

We are current utilizing `MongoDb` so be sure to follow the setup instructions in the setup guide.

### Connection String
The default connection string must utilize the `string.format` method (already implemented). Both the `user` and `password`
values are stored as `user secrets` meaning they ARE NOT PUT IN SOURCE CONTROL. This is for security reasons.

> `Database:Host`

> `Database:User`

> `Database:Password`

For your local environment you can run the following commands to set up your connection string!

```bash
dotnet user-secrets set "Database:User" "whatever user you used during setup" # such as root
```

```bash
dotnet user-secrets set "Database:Password" "whatever password you used during setup"
```

```bash
# you might need to set a port (like localhost:1234 )
# where 1234 = whichever port MongoDb is listening on
dotnet user-secrets set "Database:Host" "localhost:27017"
```

Production
-----

A separate docker image is utilized in production. This should match the database type we're using during development. So if we're using MongoDb our image should be MongoDb.

New Entities for Database
-----
1. Define the class(es) within the [Domain.Models](../../DevryDeveloperClub.Domain/Models/) portion of the project. Feel free to add subfolders for organizational purposes. Utilize existing entities as reference.

New Service/Controller
-----
If you need a reference to the database - our architecture leverages `Dependency Injection` or `DI`. This means within the constructor
of your object you can specify all the things you need! If you need to talk to the database you need to utilize [IBaseDbService](../../DevryDeveloperClub.Infrastructure/Data/IBaseDbService.cs). 
If your service requires [Tag](../../DevryDeveloperClub.Domain/Models/Tag.cs) you will simply use

```c#
using DevryDeveloperClub.Infrastructure.Interfaces;

public class MyNewService
{
    private readonly IBaseDbService<Tag> _service;
    
    public MyNewService(IBaseDbService<Tag> service)
    {
        _service = service;
    }
    
    public Task MyAwesomeNewThing()
    {
        return Task.CompletedTask;
    }
}
```

Services are things that are geared towards a specified task... they contain the business logic for our application. Meanwhile
controllers are the endpoints of our application. AKA - the URL users go to in order to interact with our application.

Please reference other items within the solution to see how this is done. 

Services belong in a folder called [Services](../Services)

A controller belongs in the [Controllers](../Controllers) folder

[Back](../../Readme.md)