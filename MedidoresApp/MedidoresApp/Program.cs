using MedidoresApp.Hilos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedidoresApp
{
    partial class Program
    {
        static void Main(string[] args)
        {
            
            int puerto = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);
            // Console.WriteLine("Iniciando hilo del server socket");
            HiloServer hiloServer = new HiloServer(puerto);
            Thread t = new Thread(new ThreadStart(hiloServer.Ejecutar));                     
            t.IsBackground = true;
            t.Start();
            while (Menu()) ;
            
        }
    }
}
