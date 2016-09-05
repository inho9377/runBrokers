using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FreeNet
{
	class CListener
	{
        // 비동기 Accept를 위한 EventArgs.
		SocketAsyncEventArgs accept_args;

		Socket listen_socket;

        // Accept처리의 순서를 제어하기 위한 이벤트 변수.
		AutoResetEvent flow_control_event;

        // 새로운 클라이언트가 접속했을 때 호출되는 콜백.
		public delegate void NewclientHandler(Socket client_socket, object token);
		public NewclientHandler callback_on_newclient;

        public CListener()
        {
			this.callback_on_newclient = null;
        }

		public void start(string host, int port, int backlog)
		{
			this.listen_socket = new Socket(AddressFamily.InterNetwork,
				SocketType.Stream, ProtocolType.Tcp);

			IPAddress address;
			if (host == "0.0.0.0")
			{
				address = IPAddress.Any;
			}
			else
			{
				address = IPAddress.Parse(host);
			}
			IPEndPoint endpoint = new IPEndPoint(address, port);

			try
			{
				listen_socket.Bind(endpoint);
				listen_socket.Listen(backlog);

				this.accept_args = new SocketAsyncEventArgs();
				this.accept_args.Completed += new EventHandler<SocketAsyncEventArgs>(on_accept_completed);

				Thread listen_thread = new Thread(do_listen);
				listen_thread.Start();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

        /// <summary>
        /// 루프를 돌며 클라이언트를 받아들입니다.
        /// 하나의 접속 처리가 완료된 후 다음 accept를 수행하기 위해서 event객체를 통해 흐름을 제어하도록 구현되어 있습니다.
        /// </summary>
		void do_listen()
		{
            this.flow_control_event = new AutoResetEvent(false);

			while (true)
			{
				this.accept_args.AcceptSocket = null;

				bool pending = true;
				try
				{
					pending = listen_socket.AcceptAsync(this.accept_args);
				}
				catch (Exception e)
				{
					continue;
				}
                
				if (!pending)
				{
					on_accept_completed(null, this.accept_args);
				}

				// 클라이언트 접속 처리가 완료되면 이벤트 객체의 신호를 전달받아 다시 루프를 수행하도록 합니다.
				this.flow_control_event.WaitOne();
                
			}
		}

        /// <summary>
        /// AcceptAsync의 콜백 매소드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">AcceptAsync 매소드 호출시 사용된 EventArgs</param>
		void on_accept_completed(object sender, SocketAsyncEventArgs e)
		{
            if (e.SocketError == SocketError.Success)
            {
                // 새로 생긴 소켓을 보관해 놓은뒤~
                Socket client_socket = e.AcceptSocket;

                // 다음 연결을 받아들인다.
                this.flow_control_event.Set();
                
                if (this.callback_on_newclient != null)
                {
                    this.callback_on_newclient(client_socket, e.UserToken);
                }

				return;
            }

			// 다음 연결을 받아들인다.
            this.flow_control_event.Set();
		}
        
	}
}
