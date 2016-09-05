using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    using FreeNet;
    using System.Threading;


    class CGameServer
    {
        object operation_lock;
        Queue<CPacket> user_operations;

        Thread logic_thread;
        
        AutoResetEvent loop_event;

        public CRoomManager room_manager { get; private set; }
        public DB data_base { get; private set; }

        public CGameServer()
        {
            this.operation_lock = new object();
            this.loop_event = new AutoResetEvent(false);
            this.user_operations = new Queue<CPacket>();
            
            this.room_manager = new CRoomManager();

            this.data_base = new DB();
            data_base.InitDB("localhost", "runbroker", "root", "741963");

            this.logic_thread = new Thread(gameloop);
            this.logic_thread.Start();
            
        }


        void gameloop()
        {
            while (true)
            {
                foreach(CGameRoom room in room_manager.game_rooms)
                {
                    room.Update();
                }

                CPacket packet = null;
                lock (this.operation_lock)
                {
                    if (this.user_operations.Count > 0)
                    {
                        packet = this.user_operations.Dequeue();
                    }
                }

                if (packet != null)
                {
                    // 패킷 처리.
                    process_receive(packet);
                }

                // 더이상 처리할 패킷이 없으면 스레드 대기.
                if (this.user_operations.Count <= 0)
                {
                    this.loop_event.WaitOne();
                }
            }
        }

        public void enqueue_packet(CPacket packet, CGameUser user)
        {
            lock (this.operation_lock)
            {
                this.user_operations.Enqueue(packet);
                this.loop_event.Set();
            }
        }

        void process_receive(CPacket msg)
        {
            msg.owner.process_user_operation(msg);
        }

        public void user_disconnected(CGameUser user)
        {
            CPacket msg = CPacket.create((short)PROTOCOL.USER_EXIT);
            msg.push(user.player.player_index);

            if (user.player.player_scene == Common.PLAYER_SCENE.WAIT_ROOM)
                user.wait_room.broadcast(msg, user.player);

            else if (user.player.player_scene == Common.PLAYER_SCENE.GAME_ROOM)
                user.battle_room.broadcast(msg, user.player);
        }

    }
}
