using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using FreeNet;

namespace Logic
{
    delegate void MyDelegate(CPacket msg);
    class PacketProcess
    {

        public Dictionary<PROTOCOL, MyDelegate> Process_Func_Dict;
        CGameUser owner;


        public void Init(CGameUser user)
        {

            owner = user;
            Process_Func_Dict = new Dictionary<PROTOCOL, MyDelegate>();
            Process_Func_Dict.Add(PROTOCOL.LOGIN_REQ, login_req);
            Process_Func_Dict.Add(PROTOCOL.REGISTER_REQ, registerUser);
            Process_Func_Dict.Add(PROTOCOL.TYPE_CHANGE_REQ, TypeChangeReq);
            Process_Func_Dict.Add(PROTOCOL.COLOR_CHANGE_REQ, ColorChangeReq);
            Process_Func_Dict.Add(PROTOCOL.READY_REQ, ReadyReq);
            Process_Func_Dict.Add(PROTOCOL.PLAYER_POSITION_REQ, PlayerPosReq);
            Process_Func_Dict.Add(PROTOCOL.GAME_RESULT, GameResult);
            Process_Func_Dict.Add(PROTOCOL.SKILL_USE_REQ, SkillUse);
            Process_Func_Dict.Add(PROTOCOL.TELEPORT_REQ, TeleportUse);
            Process_Func_Dict.Add(PROTOCOL.ITEM_USE_REQ, ItemUse);
            Process_Func_Dict.Add(PROTOCOL.TELEPORT_ITEM_REQ, TeleporItemtUse);
            Process_Func_Dict.Add(PROTOCOL.LOADING_COMPLETE_REQ, LoadingComplete);
        }

        public void Process(CPacket msg)
        {

            PROTOCOL protocol = (PROTOCOL)msg.pop_protocol_id();
            Console.WriteLine("protocol id " + protocol);
            Process_Func_Dict[protocol](msg);


        }

        

        void login_req(CPacket msg)
        {
            Console.WriteLine("LOGIN_REQ");
            string id = msg.pop_string();
            string pwd = msg.pop_string();

            if(!Program.game_main.data_base.IsUserExist(id, pwd))
            {
                CPacket send_fail_msg = CPacket.create((short)PROTOCOL.LOGIN_FAIL);
                this.owner.send(send_fail_msg);
                return;
            }


            this.owner.id = id;
            //Program.game_main.data_base.RegisterUser(id, pwd);

            CWaitRoom room = Program.game_main.room_manager.FindLeisureWaitRoom(this.owner);

            if(room == null)
            {
                CPacket fullRoomMsg = CPacket.create((short)PROTOCOL.FULL_ROOM);
                this.owner.send(fullRoomMsg);
                return;
                //full room 보냄
            }

            List<CPlayer> other_players = room.playerList;

            //중복 유저 존재하면 로그인 불가
            foreach(CPlayer player in other_players)
            {
                if(player.owner.id == id)
                {
                    CPacket send_fail_msg = CPacket.create((short)PROTOCOL.LOGIN_FAIL);
                    this.owner.send(send_fail_msg);
                    return;
                }
            }


            CPacket send_msg = CPacket.create((short)PROTOCOL.LOGIN_RES);
            send_msg.push(owner.player.player_index);
            send_msg.push((Int16)other_players.Count);

            foreach(CPlayer player in other_players)
            {
                //if (player.player_index == this.owner.player.player_index)
                //    continue;
                send_msg.push(player.player_index);
                send_msg.push(player.owner.id);
                send_msg.push((byte)player.player_color);
                send_msg.push((byte)player.player_type);
                send_msg.push((Int16)player.owner.wait_room.GetReadyState(player));
            }

            owner.send(send_msg);

            //waitroom처리해야함
            //login ntf보내야함
            room.EnterWaitRoom(owner);
            owner.enter_wait_room(owner.player, room);

            CPacket login_msg = CPacket.create((short)PROTOCOL.LOGIN_NTF);
            login_msg.push(owner.player.player_index);
            login_msg.push(id);

            room.broadcast(login_msg, owner.player);

        }

        void registerUser(CPacket msg)
        {
            string id = msg.pop_string();
            string pwd = msg.pop_string();
            CPacket send_msg = CPacket.create((short)PROTOCOL.REGISTER_RES);
            if(Program.game_main.data_base.RegisterUser(id, pwd))
            {
                //Register success
                send_msg.push((Int16)1);
            }
            else
            {
                //Already Exist id
                send_msg.push((Int16)0);
            }

            owner.send(send_msg);
        }

        void TypeChangeReq(CPacket msg)
        {
            Common.PlayerType type = (Common.PlayerType)msg.pop_byte();
            Console.WriteLine(owner.player.player_index + " : change type : " + type);

            owner.player.player_type = type;

            CPacket type_send_msg = CPacket.create((short)PROTOCOL.TYPE_CHANGE_NTF);
            type_send_msg.push(owner.player.player_index);
            type_send_msg.push((byte)type);
            owner.wait_room.broadcast(type_send_msg);

        }

        void ColorChangeReq(CPacket msg)
        {
            Common.PlayerColor color = (Common.PlayerColor)msg.pop_byte();
            Console.WriteLine(owner.player.player_index + " : change color : " + color);

            owner.player.player_color = color;
            CPacket color_send_msg = CPacket.create((short)PROTOCOL.COLOR_CHANGE_NTF);
            color_send_msg.push(owner.player.player_index);
            color_send_msg.push((byte)color);

            owner.wait_room.broadcast(color_send_msg);

        }

        void ReadyReq(CPacket msg)
        {
            Console.WriteLine(owner.player.player_index + " :  READY! ");

            owner.wait_room.SetReady(owner.player);

            CPacket ready_msg = CPacket.create((short)PROTOCOL.READY_NTF);
            ready_msg.push(owner.player.player_index);
            owner.wait_room.broadcast(ready_msg);

            if (owner.wait_room.playerList.Count == 1)
                return;

            if(owner.wait_room.isGameStart())
            {
                CGameRoom room = Program.game_main.room_manager.CreateGameRoom(owner.wait_room.playerList);
                List<byte> players_index = new List<byte>();
                CPacket send_msg = CPacket.create((short)PROTOCOL.GAME_START);

                foreach (CPlayer user in owner.wait_room.playerList)
                {
                    room.EnterGameRoom(user.owner);
                    user.owner.enter_room(user, room);
                    send_msg.push(user.player_index);
                }
                owner.wait_room.GameStart();
                
                room.broadcast(send_msg);
                //room.GameStart(60);
            }


        }
        
        void PlayerPosReq(CPacket msg)
        {
            float positionX = msg.pop_float();
            float positionY = msg.pop_float();
            float positionZ = msg.pop_float();
            float speed = msg.pop_float();
            CPacket send_msg = CPacket.create((short)PROTOCOL.PLAYER_POSITION_NTF);
            send_msg.push(owner.player.player_index);
            send_msg.push(positionX);
            send_msg.push(positionY);
            send_msg.push(positionZ);
            send_msg.push(speed);
            owner.battle_room.broadcast(send_msg, owner.player);
        }

        void GameResult(CPacket msg)
        {
            int isWin = msg.pop_int16();
            if (isWin == 2)
                return;

            Program.game_main.data_base.UpdateGameResult(owner.id, isWin == 1);
        }

        void SkillUse(CPacket msg)
        {
            CPacket send_msg = CPacket.create((short)PROTOCOL.SKILL_USE_NTF);
            send_msg.push(owner.player.player_index);
            owner.battle_room.broadcast(send_msg, owner.player);
        }

        void TeleportUse(CPacket msg)
        {
            float positionX = msg.pop_float();
            float positionZ = msg.pop_float();
            CPacket send_msg = CPacket.create((short)PROTOCOL.TELEPORT_NTF);
            send_msg.push(owner.player.player_index);
            send_msg.push(positionX);
            send_msg.push(positionZ);
            owner.battle_room.broadcast(send_msg, owner.player);
        }

        void ItemUse (CPacket msg)
        {
            byte item_index = msg.pop_byte();

            CPacket send_msg = CPacket.create((short)PROTOCOL.ITEM_USE_NTF);
            send_msg.push(owner.player.player_index);
            send_msg.push(item_index);
            owner.battle_room.broadcast(send_msg, owner.player);
        }

        void TeleporItemtUse(CPacket msg)
        {
            
            byte item_index = msg.pop_byte();
            float positionX = msg.pop_float();
            float positionZ = msg.pop_float();
            CPacket send_msg = CPacket.create((short)PROTOCOL.TELEPORT_ITEM_NTF);
            send_msg.push(owner.player.player_index);
            send_msg.push(item_index);
            send_msg.push(positionX);
            send_msg.push(positionZ);
            owner.battle_room.broadcast(send_msg, owner.player);
        }


        void LoadingComplete(CPacket msg)
        {
            if(owner.battle_room.isLoadComplete())
            {
                CPacket send_msg = CPacket.create((short)PROTOCOL.LOADING_COMPLETE_NTF);
                owner.battle_room.broadcast(send_msg);
                owner.battle_room.GameStart(60);
            }


        }
    }
}
