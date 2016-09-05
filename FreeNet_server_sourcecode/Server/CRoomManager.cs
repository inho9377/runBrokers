using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class CRoomManager
    {
        public List<CGameRoom> game_rooms;
        List<CWaitRoom> wait_rooms;

        public CRoomManager()
        {
            // 방은 메모리풀로 가지고 있는다.
            this.game_rooms = new List<CGameRoom>();
            this.wait_rooms = new List<CWaitRoom>();

            for(int i=0; i<Common.MAX_ROOM_COUNT; i++)
            {
                CGameRoom gameroom = new CGameRoom();
                game_rooms.Add(gameroom);
                CWaitRoom waitroom = new CWaitRoom();
                wait_rooms.Add(waitroom);
            }
        }
        

        /// <summary>
        /// 매칭을 요청한 유저들을 넘겨 받아 게임 방을 생성한다.
        /// 혹은 적절한 방을 넘겨준다.
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        public CGameRoom CreateGameRoom(List<CPlayer> users)
        {

            int current_game_room_index = 0;
            if(current_game_room_index > Common.MAX_ROOM_COUNT)
            {
                Console.WriteLine("FULL ROOM");
                return null;
            }
            
            CGameRoom room = game_rooms[current_game_room_index];

            return room;
        }

        public void remove_game_room(CGameRoom room)
        {
            this.game_rooms.Remove(room);
        }
        
        public void exit_wait_room(CGameUser user, CWaitRoom room)
        {

        }

        public CWaitRoom FindLeisureWaitRoom(CGameUser user)
        {
            int current_wait_room_index = 0;
            while(wait_rooms[current_wait_room_index].playerList.Count == Common.MAX_ROOM_USER_COUNT)
            {
                current_wait_room_index++;

                if (current_wait_room_index >= Common.MAX_ROOM_COUNT)
                {
                    Console.WriteLine("FULL ROOM");
                    return null;
                }
            }


            CWaitRoom room = wait_rooms[current_wait_room_index];

            return room;
           
        }

        public void remove_wait_room(CWaitRoom room)
        {
            this.wait_rooms.Remove(room);
        }
    }
}
