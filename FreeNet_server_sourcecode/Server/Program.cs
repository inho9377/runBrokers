using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeNet;


namespace Logic
{
    class Program
    {
        static List<CGameUser> userlist;
        public static CGameServer game_main = new CGameServer();

        static void Main(string[] args)
        {
            CPacketBufferManager.initialize(200);
            userlist = new List<CGameUser>();

            CNetworkService service = new CNetworkService();
            service.session_created_callback += on_session_created;

            service.initialize();
            service.listen("0.0.0.0", 7979, 100);

            Console.WriteLine("SERVER Start!");

            while (true)
            {
                string input = Console.ReadLine();
                System.Threading.Thread.Sleep(1000);

            }

            Console.ReadKey();
        }


        static void on_session_created(CUserToken token)
        {
            CGameUser user = new CGameUser(token);
            lock (userlist)
            {
                userlist.Add(user);
            }
        }

        public static void remove_user(CGameUser user)
        {
            lock (userlist)
            {
                userlist.Remove(user);
                game_main.user_disconnected(user);
                

                CGameRoom room = user.battle_room;
                
            }
        }
    }
}
