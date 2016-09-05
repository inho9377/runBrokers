using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeNet;

namespace Logic
{
    /// <summary>
	/// 하나의 session객체를 나타낸다.
	/// </summary>
	public class CGameUser : IPeer
    {
        CUserToken token;
        public string id;
        public CGameRoom battle_room { get; private set; }
        public CWaitRoom wait_room { get; private set; }

        public CPlayer player { get; private set; }

        PacketProcess packetProcess;

        public static byte total_player_index = 0;
        
        

        public CGameUser(CUserToken token)
        {
            this.token = token;
            this.token.set_peer(this);
            packetProcess = new PacketProcess();
            packetProcess.Init(this);
            player = new CPlayer(this, total_player_index);
            total_player_index++;
        }

        void IPeer.on_message(Const<byte[]> buffer)
        {
            byte[] clone = new byte[1024];
            Array.Copy(buffer.Value, clone, buffer.Value.Length);
            CPacket msg = new CPacket(clone, this);
            Program.game_main.enqueue_packet(msg, this);
        }

        void IPeer.on_removed()
        {
            Console.WriteLine("The client disconnected.");

            if(wait_room != null)
            {
                wait_room.ExitWaitRoom(this);
            }
            if(battle_room != null)
            {
                battle_room.ExitGameRoom(this);
            }

            Program.remove_user(this);
        }

        public void send(CPacket msg)
        {
            this.token.send(msg);
        }

        void IPeer.disconnect()
        {
            this.token.socket.Disconnect(false);
        }

        void IPeer.process_user_operation(CPacket msg)
        {
            packetProcess.Process(msg);
        }

        
        public void enter_wait_room(CPlayer player, CWaitRoom room)
        {
            this.player = player;
            this.wait_room = room;
            this.player.player_scene = Common.PLAYER_SCENE.WAIT_ROOM;
        }

        public void enter_room(CPlayer player, CGameRoom room)
        {
            this.player = player;
            this.battle_room = room;
            this.player.player_scene = Common.PLAYER_SCENE.GAME_ROOM;
        }
    }
}
