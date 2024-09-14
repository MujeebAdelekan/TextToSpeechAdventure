/*******************************************************************************************
 * Text-to-Speech Adventure by Mujeeb Adelekan
 * Last updated: May 8, 2022
 * Senior Project for St. Mary's University's B.S. Program in Computer Science
 ******************************************************************************************/

// C# 'using' directives
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

// ********* GameController Class **********
namespace Text_To_Speech_Adventure
{
    class GameController
    {
        /*** GameController Attributes ***/
        // made static so that ALL instances of GameController share the same variables
        static RoomNavigation roomNavigation; // a reference to the RoomNavigation class
        static List<string> actionLog = new List<string>(); // list of actions the user has taken
        static SqlConnection sqlCon; // represents connection to SQL Server database

        /*** GameController Methods ***/
        // Main()
        static void Main(string[] args)
        {
            
            // ********************************** Connect to SQL Server ********************************************
            /* Connection String syntax:
             * For Windows auth.
             * @"Data Source=(MachineName)\(InstanceName);Initial Catalog=(DBName);Integrated Security=True;"
             */
            sqlCon = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=Room Database;Integrated Security=True;");


            // ***************************** Create a new roomNavigation object *******************************************
            roomNavigation = new RoomNavigation();

            // ***************************** Initialize Starting Room *****************************************************
            roomNavigation.currentRoom = new Room();
            
            // set up connection
           
            sqlCon.Open();

            /*** get room description ***/

            // write SQL command for the room description
            string cmdText = "SELECT Description FROM Scenario WHERE RoomState = 0";
            SqlCommand command = new SqlCommand(cmdText, sqlCon);

            //send the command text to the sql connection, and build a SqlDataReader
            SqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                roomNavigation.currentRoom.setDescription((string)dataReader.GetValue(0));
            }

            dataReader.Close(); //close the current data reader

            /*** get room name ***/
            cmdText = "SELECT Room.Name " +
                      "FROM Room, Scenario " +
                      "WHERE Room.RoomID = Scenario.RoomID AND Scenario.RoomState = 0";
            command = new SqlCommand(cmdText, sqlCon);
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                roomNavigation.currentRoom.setRoomName((string)dataReader.GetValue(0));
            }

            dataReader.Close();

            //Display the room's text
            var ctrl = new GameController();
            ctrl.AddRoomDescriptionToActionLog();
            ctrl.DisplayLoggedText();

            /*** display the options of the current scenario ***/
            roomNavigation.DisplayOptionsForCurrentScenario(0); //zero corresponds to the starting scenario

            Console.WriteLine(); //newline

            //close the connection
            sqlCon.Close();
           
        }

        // DisplayLoggedText(): display the current content of the action log
        public void DisplayLoggedText()
        {
            // join the list of strings into a singluar log
            string logAsText = string.Join("\n", actionLog.ToArray());

            // split the log into separate words
            string[] formattedText = logAsText.Split(' ');

            // store each line as an array of words-per-line (wpl) words
            int wpl = 20; 
            string[] line = new string[wpl];

            // print wpl words per line so Narrator can read each word in its entirety
            for (int i = 0; i < formattedText.Length; i++)
            {
                line[i % wpl] = formattedText[i];

                if ((i % wpl == wpl - 1) || (i == formattedText.Length - 1))
                {
                    Console.WriteLine(string.Join(" ", line));
                    Array.Clear(line, 0, wpl);
                }

            }
        }

        // AddRoomDescriptionToActionLog() (previously named DisplayRoomText()):
        //     display the current room's description by adding it to the action log
        public void AddRoomDescriptionToActionLog()
        {
            actionLog.Clear(); 

            //add the current room description to the action log
            Console.WriteLine("Your current location is the " + roomNavigation.currentRoom.getRoomName() + "\n");
            AddToActionLog(roomNavigation.currentRoom.getDescription() + "\n");
        }

        // AddToActionLog(): add the description of the chosen action to the action log
        void AddToActionLog(string actionDescription)
        {
            actionLog.Add(actionDescription + "\n");
        }

        //GetSQLConnection(): get access to the SQL server connection
        public SqlConnection GetSQLConnection()
        {
            return sqlCon;
        }    
    }
}
