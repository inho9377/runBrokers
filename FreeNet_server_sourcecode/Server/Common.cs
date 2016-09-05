
using System;
namespace Logic
{
    public class Common
    {
        public const int MAX_ROOM_USER_COUNT = 4;

        public const int MAX_ROOM_COUNT = 1;
        public enum PlayerColor : byte
        {
            DEFAULT = 100,
            RED = 0,
            GREEN = 1,
            YELLOW = 2,
            PURPLE = 3,
            WHITE = 4
        };

        public const int numColor = 5;



        public enum PlayerType : byte
        {
            UNITY_CHAN = 0,
            KNIGHT = 1,
            TEDDY = 2,
            TOON_BOT = 3
        };

        public const int numType = 4;


        public enum PLAYER_SCENE : byte
        {
            TITLE,
            WAIT_ROOM,
            GAME_ROOM
        }

        public enum KeyCode : byte
        {
            LEFT = 0,
            RIGHT = 1,
            UP = 2,
            DOWN = 3,
        }




    }

}