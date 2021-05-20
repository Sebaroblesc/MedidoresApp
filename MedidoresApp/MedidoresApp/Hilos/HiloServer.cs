using SocketUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedidoresApp.Hilos
{
    public class HiloServer
    {
        private int puerto;
        private ServerSocket server;
        public HiloServer(int puerto)
        {
            this.puerto = puerto;
        }

        public void Ejecutar()
        {
            server = new ServerSocket(puerto);
            Console.WriteLine("Iniciando Server en puerto {0}", puerto);
            if (server.Iniciar())
            {
                Console.WriteLine("Servidor Iniciado");
                while (true)
                {
                    Console.WriteLine("Esperando Medidores...");
                    ClienteSocket clienteSocket = server.ObtenerCliente();
                    HiloCliente hiloCliente = new HiloCliente(clienteSocket);
                    Thread t = new Thread(new ThreadStart(hiloCliente.Ejecutar));
                    t.IsBackground = true;
                    t.Start();
                }
            }
        }
    }
}
