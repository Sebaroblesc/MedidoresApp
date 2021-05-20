using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketUtils 
{
    public class ServerSocket
    {
        private int puerto;
        private Socket servidor;

        public ServerSocket(int puerto)
        {
            this.puerto = puerto;
        }

        public bool Iniciar()
        {
            try
            {
                this.servidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.servidor.Bind(new IPEndPoint(IPAddress.Any, this.puerto));                
                this.servidor.Listen(10);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public ClienteSocket ObtenerCliente()
        {
            try
            {
                return new ClienteSocket(this.servidor.Accept());


            }
            catch (Exception ex)
            {
                return null;
            }
        }



    }
}