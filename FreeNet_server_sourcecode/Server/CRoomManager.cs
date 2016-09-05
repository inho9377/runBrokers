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

      //  int gameroom_current_index = 0;
        //int waitroom_current_index = 0;


        /// <summary>
        /// 매칭을 요청한 유저들을 넘겨 받아 게임 방을 생성한다.
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        public CGameRoom CreateGameRoom(List<CPlayer> users)
        {

            int current_game_room_index = 0;
            //while (game_rooms[current_game_room_index].players.Count != 0)
            //{
            //    current_game_room_index++;
            //}

            if(current_game_room_index > Common.MAX_ROOM_COUNT)
            {
                Console.WriteLine("FULL ROOM");
                return null;
            }



            CGameRoom room = game_rooms[current_game_room_index];

            return room;
            /*
            if((gameroom_current_index == 0) ||
                (game_rooms[gameroom_current_index].players.Count == Common.MAX_ROOM_USER_COUNT)
                )
            {
                CGameRoom battleroom = new CGameRoom();
                gameroom_current_index++;
                this.game_rooms.Add(battleroom);

            }


            game_rooms[gameroom_current_index].EnterGameRoom(user);
            */
            // 게임 방을 생성하여 입장 시킴.

            //battleroom.enter_gameroom(user_list);

            // 방 리스트에 추가 하여 관리한다.

        }

        public void remove_game_room(CGameRoom room)
        {
            //room.destroy();
            this.game_rooms.Remove(room);
        }

        public void find_game_room(byte playerIndex)
        {
            //해당 유저가 있는 방을 리턴
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
                /*
            if ((waitroom_current_index == 0) ||
                (wait_rooms[waitroom_current_index].playerList.Count == Common.MAX_ROOM_USER_COUNT)
                )
            {
                //방 나가면 어카지..=ㅂ=;
                //결원 생기면 현재 index를 해당 index로하고..
                //다음 index의 인원 체크해서 빈 방일때까지 가는식으로***
                CWaitRoom room = new CWaitRoom();
               
                this.wait_rooms.Add(room);
                wait_rooms[waitroom_current_index].EnterWaitRoom(user);
                waitroom_current_index++;
                return wait_rooms[waitroom_current_index-1];
            }
            else
            {
                wait_rooms[waitroom_current_index].EnterWaitRoom(user);
                return wait_rooms[waitroom_current_index];
            }

           */
           
        }

        public void remove_wait_room(CWaitRoom room)
        {
            //room.destroy();
            this.wait_rooms.Remove(room);
        }
    }
}
