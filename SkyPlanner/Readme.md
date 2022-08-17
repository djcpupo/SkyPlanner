Prerequisites

1- Microsoft Visual Studio 2019 or higher (2022 Preferred)
    - VS can be installed with the Web development features checked

2- Microsoft SQL Server 2014 or higher
    - SQL Server can be installed with the default options, only get sure you have checked the "Microsoft SQL Server Management Studio" or "Development Tools"
    - Create your own password for the "sa" user admin to be used in the Web API setup

--------------------------------------------------------------

Step-by-Step

1- Fork the project from Github (https://github.com/djcpupo/SkyPlanner)

2- Excecute the Script/SkyPlanner.sql script in the local SQL Server
    - Once you have the repo, open the Microsoft SQL Server Management Studio and login into the SQL Server
    - Open the file located in Script/SkyPlanner.sql and excecute it (F5), this script will create the database schema and will insert few data into Account, Contact and Product
    - The database name will be SkyPlanner

3- Open the SkyPlanner/SkyPlanner.sln file to open the project in VS

4- Setup the Connection String in the appsettings.json file with your local DB (ServerName (data source), database name (initial catalog), user (User ID), password)

    "ConnectionStrings": {
        "DefaultConnection": "data source=ServerName;initial catalog=SkyPlanner;User ID=sa;password=lamisma;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"
    }

    - If you have installed the SQL Server, the server name should be your PC-Name
    - The initial catalog is the same SkyPlanner db 
    - The User ID is "sa" because it is the default user admin of your SQL Server
    - The password is the one you configured when installing the SQL Server for the "sa" user

5- Execute the app in VS (F5)

Swagger

    - When you excecute the app then a Browser tab or window will be open
    - The tab will show all services
    - Every service in that tab will have a small description, the same description is in the source code
    - If click one of those services the you can Try it with the "Try it out" button and you will see the service response y the response box
    - By default the response box have a sample response but not a real one