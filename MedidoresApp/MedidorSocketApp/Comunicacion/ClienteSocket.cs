using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MedidorSocketApp.Comunicacion
{
    public class ClienteSocket
    {
        private string ip;
        private int puerto;
        private Socket comServidor;
        private StreamReader reader;
        private StreamWriter writer;

        public ClienteSocket(string ip, int puerto)
        {
            this.puerto = puerto;
            this.ip = ip;
        }

        public bool Conectar() {
            try
            {
                this.comServidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ip), puerto);
                this.comServidor.Connect(endpoint);
                Stream stream = new NetworkStream(this.comServidor);
                this.reader = new StreamReader(stream);
                this.writer = new StreamWriter(stream);
                return true;
            }catch(IOException ex)
            {
                return false;
            }
        }
        public bool Escribir(string lectura)
        {
            try
            {
                this.writer.WriteLine(lectura);
                this.writer.Flush();
                return true;
            }
            catch (IOException ex)
            {
                return false;
            }
        }

        public string Leer()
        {
            try
            {
                return this.reader.ReadLine().Trim();
            }
            catch (IOException ex)
            {
                return null;
            }
        }

        public void Desconectar()
        {
            this.comServidor.Close();
        }
    }
}