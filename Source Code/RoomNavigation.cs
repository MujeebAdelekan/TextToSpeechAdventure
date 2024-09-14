//C# 'using' directives
using System;
using System.Data.SqlClient;

// ********* RoomNavigation Class **********
namespace Text_To_Speech_Adventure
{
    class RoomNavigation
    {
        /*** RoomNavigation Attributes ***/
        public Room currentRoom; //holds the room the player is currently in
        GameController controller; // a reference to the GameController class
        static int scenario; //current room state  the player is in
        string[] options = new string[4]; // array that holds the current options the user can perform
        SqlConnection sqlCon; // represents connection to SQL Server database

        

        /*** RoomNavigation Methods ***/
        // RoomNavigation constructor
        public RoomNavigation()
        {
            currentRoom = new Room();
            controller = new GameController();
            sqlCon = controller.GetSQLConnection();
        }

        // DisplayOptionsForCurrentScenario(): print the options for the current scenario (room state)
        public void DisplayOptionsForCurrentScenario(int roomState)
        {
            // initialize scenario upon starting the game
            if (roomState == 0) 
            { 
                scenario = roomState;
            }

            // variables used for SQL Queries
            string cmdText;
            SqlCommand command;
            SqlDataReader dataReader;

            string optLine;      // full option line to be printed
            string optDesc = ""; // string representing option description
            char key;            // key to be pressed 

            Console.WriteLine("INSTRUCTIONS: Using the D, F, J, and K keys, press the key corresponding with your action of choice.");
            Console.WriteLine("Press Q at any time to close the game.");
            Console.WriteLine("Your options are:");
            for (int i = 1; i <= 4; i++)
            {
                // write SQL command for the option description
                cmdText = "SELECT Opt" + i + "Desc FROM Scenario WHERE RoomState = " + scenario;
                command = new SqlCommand(cmdText, sqlCon);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    optDesc = (string)dataReader.GetValue(0);    
                }

                //Print to terminal
                switch (i)
                {
                    case 1:
                        key = 'D';
                        break;
                    case 2:
                        key = 'F';
                        break;
                    case 3:
                        key = 'J';
                        break;
                    case 4:
                        key = 'K';
                        break;
                    default:
                        throw new ArgumentException("Queried option does not exist");
                }
                optLine = key + ". " + optDesc;
                Console.WriteLine(optLine);

                options[i - 1] = optLine;


                dataReader.Close();   
            }

            AcceptInput();
        }

        // AcceptInput(): takes in the integer representation of the option chosen by the user
        void AcceptInput()
        {
            int choice = 0;
            ConsoleKeyInfo cki;

            do
            {
                cki = Console.ReadKey(); //read the user's input
                switch (cki.Key)
                {
                    case ConsoleKey.D:
                        choice = 1;
                        Console.WriteLine("\nYou picked " + options[choice - 1]);
                        Console.WriteLine("Press another key to change your option or press Spacebar to confirm.");
                        break;

                    case ConsoleKey.F:
                        choice = 2;
                        Console.WriteLine("\nYou picked " + options[choice - 1]);
                        Console.WriteLine("Press another key to change your option or press Spacebar to confirm.");
                        break;

                    case ConsoleKey.J:
                        choice = 3;
                        Console.WriteLine("\nYou picked " + options[choice - 1]);
                        Console.WriteLine("Press another key to change your option or press Spacebar to confirm.");
                        break;

                    case ConsoleKey.K:
                        choice = 4;
                        Console.WriteLine("\nYou picked " + options[choice - 1]);
                        Console.WriteLine("Press another key to change your option or press Spacebar to confirm.");
                        break;

                    case ConsoleKey.Spacebar:
                        if (choice != 0)
                        {
                            Console.WriteLine("Confirming action: " + options[choice - 1] + "\n");
                        }
                        else
                        {
                            Console.WriteLine("\nInvalid input. Press the key corresponding with your action of choice.\n");
                            DisplayOptionsForCurrentScenario(scenario);
                        }
                        break;

                    case ConsoleKey.Q:
                        // close the game
                        Console.WriteLine("Closing Text-to-Speech Adventure.");
                        Environment.Exit(0);
                        break;

                    // Allow the user to change volume and brightness without printing "Invalid Input"
                    case ConsoleKey.VolumeDown:
                    case ConsoleKey.VolumeUp:
                    case ConsoleKey.VolumeMute:
                    case (ConsoleKey)255: // brightness setting
                        break;

                    // CHEAT CODE FOR CS SYMPOSIUM PRESENTATION
                    case ConsoleKey.Z:
                        if (scenario == 29) //Arriving in the Dark Caverns
                        {
                            Console.WriteLine("Skipping to the Aqueduct Puzzle");
                            SkipToSecondPuzzle();
                        }
                        break;
                    default:
                        Console.WriteLine("\nInvalid input. Press the key corresponding with your action of choice.\n");
                        DisplayOptionsForCurrentScenario(scenario);
                        break;
                }

            } while (cki.Key != ConsoleKey.Spacebar);

            RespondToInput(choice);
        }

        // RespondToInput()
        void RespondToInput(int action)
        {
            // variables used for SQL Queries
            string cmdText;
            SqlCommand command;
            SqlDataReader dataReader;

            cmdText = "SELECT Opt" + action + "Res FROM Scenario WHERE RoomState = " + scenario;
            command = new SqlCommand(cmdText, sqlCon);
            dataReader = command.ExecuteReader();

            while (dataReader.Read())
            { 
                scenario = (int)dataReader.GetValue(0);
            }

            dataReader.Close();
 
            // update room description 
            cmdText = "SELECT Description FROM Scenario WHERE RoomState = " + scenario;
            command = new SqlCommand(cmdText, sqlCon);
            dataReader = command.ExecuteReader();
                
            while(dataReader.Read())
            {
                currentRoom.setDescription((string)dataReader.GetValue(0));
            }

            dataReader.Close();

            // update room name
            cmdText = "SELECT Room.Name " +
                        "FROM Room, Scenario " +
                        "WHERE Room.RoomID = Scenario.RoomID AND Scenario.RoomState = " + scenario;
            command = new SqlCommand(cmdText, sqlCon);
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                currentRoom.setRoomName((string)dataReader.GetValue(0));
            }

            dataReader.Close();

            controller.AddRoomDescriptionToActionLog();
            controller.DisplayLoggedText();
             
            // determine if the player has finished the game
            if (scenario != 1999)
            {
                DisplayOptionsForCurrentScenario(scenario);
            }
            else
            {
                Environment.Exit(0);
            }

            

        }

        // SkipToSecondPuzzle(): Brings the user to the aqueduct puzzle after the user enters the Dark Caverns
        // /*** THIS IS A CHEAT FOR THE CS SYMPOSIUM ***/
        void SkipToSecondPuzzle()
        {
            // set scenario to 35 which will bring the user to the second puzzle
            scenario = 35;

            // variables used for SQL Queries
            string cmdText;
            SqlCommand command;
            SqlDataReader dataReader;

            // update room description 
            cmdText = "SELECT Description FROM Scenario WHERE RoomState = " + scenario;
            command = new SqlCommand(cmdText, sqlCon);
            dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                currentRoom.setDescription((string)dataReader.GetValue(0));
            }

            dataReader.Close();

            // update room name
            cmdText = "SELECT Room.Name " +
                     "FROM Room, Scenario " +
                     "WHERE Room.RoomID = Scenario.RoomID AND Scenario.RoomState = " + scenario;
            command = new SqlCommand(cmdText, sqlCon);
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                currentRoom.setRoomName((string)dataReader.GetValue(0));
            }

            dataReader.Close();

            controller.AddRoomDescriptionToActionLog();
            controller.DisplayLoggedText();
            DisplayOptionsForCurrentScenario(scenario);

        }

    }

}
   
