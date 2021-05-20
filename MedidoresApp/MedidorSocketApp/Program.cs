using MedidorSocketApp.Comunicacion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedidorSocketApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = ConfigurationManager.AppSettings["ip"];
            int puerto = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);

            ClienteSocket clienteSocket = new ClienteSocket(ip, puerto);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Conectandose al servidor {0} en el puerto {1}", ip, puerto);
            if (clienteSocket.Conectar())
            {
                Console.WriteLine("Cliente Conectado.");
                Console.WriteLine(clienteSocket.Leer());
                string prueba = Console.ReadLine();
                clienteSocket.Escribir(prueba);
                string comprobar = clienteSocket.Leer();                
                if (comprobar.Contains("WAIT")){
                    Console.WriteLine("Número Medidor|Fecha|Tipo|Valor|Estado (opcional)|UPDATE");                    
                    string input = Console.ReadLine();
                    clienteSocket.Escribir(input);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(clienteSocket.Leer());
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Solicitud rechazada");
                }
                try
                {
                    Console.WriteLine(clienteSocket.Leer());
                }
                catch (NullReferenceException ex)
                {

                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error de conexión");
            }
        }
    }
}

       


   