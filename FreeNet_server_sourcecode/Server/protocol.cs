using System;

namespace Logic
{
    public enum PROTOCOL : short
    {
        

        /*
         * string Id
         * string password
         */
        LOGIN_REQ = 2,



        LOGIN_FAIL = 3,

        /*
         * byte controller_index
         * int player_num
         * foreach(player_num)
            byte index
            string id
            playercolor
            playertype
            bool isReady
         */
        LOGIN_RES = 4,

        /*
        * byte player_index
        * string player_id
        */
        LOGIN_NTF = 5,

        /*
        * string id
        * string password
        */
        REGISTER_REQ = 6,

        /*
        * bool isSuccess 1:registersuccess, 0:registerfail (Already Exist id)
        */
        REGISTER_RES = 7,


        FULL_ROOM = 8,

        /*
         * PlayerType change_type
         */
        TYPE_CHANGE_REQ = 10,
        

        /*
        * byte player_index
        * PlayerType change_type : byte
        */
        TYPE_CHANGE_NTF = 12,

        /*
        * PlayerColor change_color  : byte
        */
        COLOR_CHANGE_REQ = 20,
        

        /*
        * byte player_index
        * PlayerColor change_color : byte
        */
        COLOR_CHANGE_NTF = 22,
        

        READY_REQ = 30,

        

        /*
        * byte player_index
        */
        READY_NTF = 32,

        /*
        * byte player1index
        * byte player2index
        * byte player3index
        * byte player4index
        */

        GAME_START = 40,



        /*
         * int playTimeSec
         */
        PLAY_TIME = 80,
        

        TIME_OVER = 82,

        /*
         * float player_positionX
         * float player_positionY
         * float player_positionZ
         * float player_speed
         */
        PLAYER_POSITION_REQ = 90,

        /*
         *  byte player_index
        * float player_positionX
        * float player_positionY
        * float player_positionZ
        * float player_speed
        */
        PLAYER_POSITION_NTF = 91,


        /*
        * byte player_index
        */
        USER_EXIT = 100,


        /*
        * int isWin (1=win, 0=lose)
        *
        */
        GAME_RESULT = 110,

        
        SKILL_USE_REQ = 120,

        /*
         * byte player_index
         */
        SKILL_USE_NTF = 121,

        /*
         * float positionX
         * float positionZ
         */
        TELEPORT_REQ = 122,

        /*
         *byte player_index
         * float positionX
         * float positionZ
         */
        TELEPORT_NTF = 123,

        /*
         * byte item_index
         */
        ITEM_USE_REQ = 130,

        /*
         * byte player_index
         * byte item_index
         */
        ITEM_USE_NTF = 131,

        /*
         * byte item_index
      * float positionX
      * float positionZ
      */
        TELEPORT_ITEM_REQ = 141,

        /*
         *byte player_index
         * byte item_index
         * float positionX
         * float positionZ
         */
        TELEPORT_ITEM_NTF = 142,

        
        LOADING_COMPLETE_REQ = 150,
        LOADING_COMPLETE_NTF = 151,
    }
}
