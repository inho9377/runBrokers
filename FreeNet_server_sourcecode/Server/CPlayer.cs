using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeNet;

namespace Logic
{
    public class CPlayer
    {
        public CGameUser owner { get; private set; }



        public byte player_index { get; private set; }

        public Common.PLAYER_SCENE player_scene = Common.PLAYER_SCENE.TITLE;

        public Common.PlayerType player_type;

        public Common.PlayerColor player_color;


        public CPlayer(CGameUser user, byte player_index)
        {
            this.owner = user;
            this.player_index = player_index;

        }

        public void send(CPacket msg)
        {
            this.owner.send(msg);
            CPacket.destroy(msg);
        }

        public void send_for_broadcast(CPacket msg)
        {
            this.owner.send(msg);
        }
    }
}
