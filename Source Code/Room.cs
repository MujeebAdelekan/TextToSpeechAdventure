// ********* Room Class (data container class) **********
namespace Text_To_Speech_Adventure
{
    class Room
    {
        string description; // holds room's description
        string roomName;    // hold room's name

        /*** Room Constructors ***/
        public Room() { } //general constructor
        
        public Room(string description, string roomName) //explicit constructor
        {
            setDescription(description);
            setRoomName(roomName);
        }


        /*** Set Methods ***/
        public void setDescription(string description)
        {
            this.description = description;
        }
        
        public void setRoomName(string roomName)
        {
            this.roomName = roomName;
        }

        /*** Get Methods ***/
        public string getDescription()
        {
            return description;
        }

        public string getRoomName()
        {
            return roomName;
        }

    }
}
