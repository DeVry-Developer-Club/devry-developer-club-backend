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
dotnet user-secrets set "Database:User" "whatever user you used during setup"
```

```bash
dotnet user-secrets set "Database:Password" "whatever password you used during setup"
```

```bash
# you might need to set a port (like localhost:1234 )
# where 1234 = whichever port MongoDb is listening on
dotnet user-secrets set "Database:Host" "localhost"
```

### Create Migration
Migrations are essentially git-like source control for your data layer. They have up/down functionality that allows you to upgrade to a particular version of your code. 
Conversely, downgrade to go back. This is useful in the event something didn't work in production so you want to revert whichever schema changes were applied.

The name of your migration should be a short description of what you did i.e `AddedConfigModel`
Make sure you run this from within the `DevryDeveloperClub` folder.

```bash
dotnet ef migrations add <name of your migration>
```

Once done, you should see your migration added to the [Migrations](../Migrations/) folder.

At this point it is important to verify the generated migration looks okay PRIOR to running the next command. If needed
you can delete this new migration, update code as necessary and run the above command again.

If you are satisfied with the migration run the following to apply changes to your database.

```bash
dotnet ef database update
```

Production
-----

A separate docker image is utilized in production. This should match the database type we're using during development. So if we're using MongoDb our image should be MongoDb.

How to add to database (new entities)
-----
1. Define the class(es) within the [Domain.Models](../../DevryDeveloperClub.Domain/Models/) portion of the project. Feel free to add subfolders for organizational purposes. Utilize existing entities as reference.
2. New models will need to be added in two places.
    [IApplicationDbContext](../../DevryDeveloperClub.Infrastructure/Data/IApplicationDbContext) and [ApplicationDbContext](../../DevryDeveloperClub.Infrastructure/Data/ApplicationDbContext). Following existing implementations --> you just add a new `DbSet<T>` where T is your new class!

New Service/Controller
-----
If you need a reference to the database - our architecture leverages `Dependency Injection` or `DI`. This means within the constructor
of your object you can specify all the things you need! Just make sure one of those items is [IApplicationDbContext](../../DevryDeveloperClub.Infrastructure/Data/IApplicationDbContext).

Services are things that do a specific thing... they contain the business logic for our application. Meanwhile
controllers are the endpoints of our application. AKA - the URL users go to in order to interact with our application.

Please reference other items within the solution to see how this is done. An example is as follows

Services belong in a folder called [Services](../Services)
```c#
using DevryDeveloperClub.Infrastructure.Interfaces;

public class MyNewService
{
    private readonly IApplicationDbContext _context;
    
    public MyNewService(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public Task MyAwesomeNewThing()
    {
        return Task.CompletedTask;
    }
}
```

A controller belongs in the [Controllers](../Controllers) folder

[Back](../../Readme.md)