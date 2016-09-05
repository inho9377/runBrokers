using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeNet;
using Logic;

namespace Logic
{
    public class Helper
    {
        public static CPacket MakePacket(PROTOCOL protocol, List<dynamic> inputs)
        {
            CPacket msg = CPacket.create((short)protocol);

            if (inputs == null)
                return msg;
        
            foreach(dynamic input in inputs)
            { 
                msg.push(input);
            }


            return msg;
        }
        
    }
}
