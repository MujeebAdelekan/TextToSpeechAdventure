Go to the following links to download the tools used to develop Text-to-Speech Adventure:
To download Visual Studio 2019: 

https://visualstudio.microsoft.com/vs/older-downloads/

To download SQL Server Management Studio 2018: 

https://docs.microsoft.com/en-us/sql/ssms/release-notes-ssms?view=sql-server-ver15#previous-ssms-releases

Setting up the development environment in Visual Studio:

The source code files of the game are GameController.cs, RoomNavigation.cs, and Room.cs. 
To put the source code files in a new project, launch Visual Studio 2019 and start a 
new project. 

In the "Creating a new project" window, choose "C# Console Application"
Visual Studio will then prompt you to name your project. I recommend the developer to name 
the project “Text-to-Speech Adventure” to make it easier to find on the machine.

After the project has been created, the Visual Studio interface should be displayed. At the top of 
the window, go to "View > Solution Explorer" to open the Solution Explorer (alternatively, you 
can press Ctrl + Alt + L to open it).

The Solution Explorer should appear on the right side of the screen. 
In the Solution Explorer, right click the name of the project, and go to 
"Add > Existing Item" (alternatively, you can press Shift + Alt + A). 

This should open File Explorer on your machine. Select the GameController.cs, RoomNavigation.cs, and Room.cs files to add them 
to the project environment. 

Go to "Build > Build Solution" to compile and build the project.
This should create a .exe file in the "Text-to-Speech Adventure" Folder that
resides in the project's location.

In your project's location, go to "Text-to-Speech Adventure" (FOLDER) > obj Folder
> Debug folder. The .exe should be in the Debug folder.

Setting up the Room Database:

Text-to-Speech Adventure includes code that queries to a database local to the development machine. 
To import the Room Database on your local machine, follow the instructions in this link: 
https://docs.microsoft.com/en-us/sql/integration-services/import-export-data/start-the-sql-server-import-and-export-wizard?view=sql-server-ver15

