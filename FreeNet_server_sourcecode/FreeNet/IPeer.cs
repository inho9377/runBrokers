using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace FreeNet
{
	public interface IPeer
	{
		void on_message(Const<byte[]> buffer);

        void on_removed();


		void send(CPacket msg);


		void disconnect();


		void process_user_operation(CPacket msg);
	}
}
