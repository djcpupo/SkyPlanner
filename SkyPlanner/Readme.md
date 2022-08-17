Prerequisites
1- Microsoft Visual Studio 2019 or higher (2022 Preferred)
2- Microsoft SQL Server 2014 or higher

--------------------------------------------------------------

Step-by-Step
1- Fork the project from Github
2- Excecute the Script/SkyPlanner.sql script in the local SQL Server
3- Setup the Connection String in the appsettings.json file with your local DB (ServerName (data source), database name (initial catalog), user (User ID), password)
    "ConnectionStrings": {
        "DefaultConnection": "data source=ServerName;initial catalog=SkyPlanner;User ID=sa;password=lamisma;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"
    }
4- Execute the app