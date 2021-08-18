# Setup
[Back](../../Readme.md)

**Important to note**

The IDE of choice is of course user preference. Nothing is stopping you from utilizing your favorite code editor. 


#### Git
We will be working with [Git](https://git-scm.com/downloads) quite a bit....so... install it!!!!

#### Backend

Download and install the latest version of [Net5.0](https://dotnet.microsoft.com/download/dotnet/5.0) - x64 installer usually suffices

Few of us are using [Rider](https://www.jetbrains.com/rider/) for `C#` which is free to use if you register 
with your DeVry email! Since we are **only** working with open-source software / educational projects you're clear to use the EDU version.

Feel free to leverage [Visual Studio](https://visualstudio.microsoft.com/downloads/) if you're not comfortable with Rider

Clone the project to your machine

```shell
git clone https://github.com/DeVry-Developer-Club/devry-developer-club-backend.git
```

Open the solution using your IDE of choice and run the following command. This will be required for certain database functionality mentioned in [Database.md](Database.md)

```shall
dotnet tool install --global dotnet-ef
```

#### Frontend

You will need [node.js](https://nodejs.org/en/) installed

As well as `yarn` - which is our package management of choice. Once `node.js` is installed run the following command in either powershell or command line.

```shell
npm install --global yarn
```

Few of us are using [WebStorm](https://www.jetbrains.com/webstorm/) which is free if you register with your DeVry email. Since we're **only** working with open-source software /educational projects 
you're clear to use their EDU version.

Alternative would be [Visual Studio Code](https://code.visualstudio.com/Download)

### Database
MongoDb Community Server [download](https://fastdl.mongodb.org/windows/mongodb-windows-x86_64-5.0.2-signed.msi)

1. Setup Type: Complete
2. Service Configuration Page -- using default values --<br>
    a.Install MongoDb - checked <br>
    b. Run Service as a Network Service User <br>
    c. Service Name: `MongoDb` <br>
    d. Default options for log/data directories
3. When installation completes and it launches you should be able to hit the green connect button -- no need to enter anything into the connection string field.

[Back](../../Readme.md)