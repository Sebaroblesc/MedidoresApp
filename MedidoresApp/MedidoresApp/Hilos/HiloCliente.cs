using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedidoresModel.DAL;
using MedidoresModel.DTO;
using Newtonsoft.Json;
using SocketUtils;

namespace MedidoresApp.Hilos
{
    class HiloCliente
    {
        private ClienteSocket clienteSocket;
        private ILecturaDAL dal = LecturaDALFactory.CreateDal();
        static IMedidorConsumoDAL dalConsumo = MedidorConsumoDALFactory.CreateDal();
        static IMedidorTraficoDAL dalTrafico = MedidorTraficoDALFactory.CreateDal();

        public HiloCliente(ClienteSocket clienteSocket)
        {
            this.clienteSocket = clienteSocket;
        }

        public void Ejecutar()
        {
            bool verificado = false;

            clienteSocket.Escribir("Ingresar: Fecha (aaaa-mm-dd-hh-mm-ss)|Numero Medidor (4 números)|Tipo (Consumo o Tráfico):");
            string prueba = clienteSocket.Leer();

            string fechaV = (prueba.Split('|'))[0];            

            int medidorV;
            try
            {
                medidorV = Int32.Parse((prueba.Split('|'))[1]);
            }
            catch (Exception ex)
            {

                medidorV = 0;
            }

            string tipoV;
            try
            {
                tipoV = (prueba.Split('|'))[2];
            }
            catch (Exception)
            {
                tipoV = "";
            }
            

            DateTime fechaNew;
            try
            {
                fechaNew = DateTime.ParseExact(fechaV, "yyyy-MM-dd-HH-mm-ss",
                    System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                fechaNew = new DateTime(2021, 01, 01, 00, 00, 00);
            }              
            
            TimeSpan intervalo = DateTime.Now - fechaNew;
            
            if(tipoV == "consumo")
            {
                foreach (var medidor in dalConsumo.ObtenerMedidores())
                {
                    if (medidorV == medidor)
                    {
                        verificado = true;
                    }
                }
            }
            else if(tipoV == "trafico")
            {
                foreach (var medidor in dalTrafico.ObtenerMedidores())
                {
                    if (medidorV == medidor)
                    {
                        verificado = true;
                    }
                }
            }
            if (verificado == true && ((intervalo.TotalMinutes < 30) && (tipoV == "trafico" || tipoV == "consumo")))
            {
                clienteSocket.Escribir(DateTime.Now + "| WAIT");
                string input;
                try
                {
                    input = clienteSocket.Leer();
                }
                catch (NullReferenceException)
                {
                    input = "";
                }
                
                string[] frase = input.Split('|');
                if (frase.Length == 6)
                {
                    int medidor;
                    try
                    {
                        medidor = Int32.Parse((input.Split('|'))[0]);
                    }
                    catch (Exception)
                    {
                        medidor = 0;                       
                    }
                    string fecha;
                    try
                    {
                        fecha = (input.Split('|'))[1];
                    }
                    catch (Exception)
                    {

                        fecha = "";
                    }

                    string tipo;
                    try
                    {
                      tipo = (input.Split('|'))[2];
                    }
                    catch (Exception)
                    {
                        tipo = "";
                      
                    }                    

                    int valor;
                    try
                    {
                      valor = Int32.Parse((input.Split('|'))[3]);
                    }
                    catch (Exception)
                    {
                        valor = -1;                       
                    }
                    int estado;
                    try
                    {
                       estado = Int32.Parse((input.Split('|'))[4]);
                    }
                    catch (NullReferenceException)
                    {
                        estado = 6;
                    }                   

                    string confirmar = (input.Split('|'))[5];

                    if(confirmar != "UPDATE" || estado < -1 || estado > 2 || valor < 0 || valor > 1000 || fecha != fechaV || medidorV != medidor)
                    {
                        clienteSocket.Escribir( medidorV + "|ERROR");
                        clienteSocket.CerrarConexion();
                    }
                    else
                    {
                        if (tipo == "consumo")
                        {
                            Lectura l = new Lectura()
                            {
                                NroSerie = medidor,
                                Fecha = fechaNew,
                                Tipo = tipo,
                                Valor = valor,
                                Estado = estado                            
                            };

                            lock (dal)
                            {
                                clienteSocket.Escribir(medidorV + "|OK");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine(medidorV + "|OK");
                                Console.ResetColor();
                                dal.RegistrarLecturaConsumo(l);
                                clienteSocket.CerrarConexion();
                            }
                        }
                        else if (tipo == "trafico")
                        {
                            Lectura l = new Lectura()
                            {
                                NroSerie = medidor,
                                Fecha = fechaNew,
                                Tipo = tipo,
                                Valor = valor,
                                Estado = estado
                            };

                            lock (dal)
                            {

                                clienteSocket.Escribir(medidorV + "|OK");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine(medidorV + "|OK");
                                Console.ResetColor();
                                dal.RegistrarLecturaTrafico(l);
                                clienteSocket.CerrarConexion();
                            }
                        }
                    }
                }
                else if (frase.Length == 5)
                {
                    int medidor;
                    try
                    {
                        medidor = Int32.Parse((input.Split('|'))[0]);
                    }
                    catch (Exception)
                    {
                        medidor = 0;
                    }

                    string fecha = (input.Split('|'))[1];

                    string tipo = (input.Split('|'))[2];

                    int valor;
                    try
                    {
                        valor = Int32.Parse((input.Split('|'))[3]);
                    }
                    catch (Exception)
                    {
                        valor = -1;
                    }
                    string confirmar = (input.Split('|'))[4];

                    if (confirmar != "UPDATE" || valor < 0 || valor > 1000 || fecha != fechaV || medidorV != medidor)
                    {
                        clienteSocket.Escribir(medidorV + "|ERROR");
                        clienteSocket.CerrarConexion();
                    }
                    else
                    {
                        if (tipo == "consumo")
                        {
                            Lectura l = new Lectura()
                            {
                                NroSerie = medidor,
                                Fecha = fechaNew,
                                Tipo = tipo,
                                Valor = valor
                            };
                            lock (dal)

                            {
                                clienteSocket.Escribir(medidorV + "|OK");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine(medidorV + "|OK");
                                Console.ResetColor();
                                dal.RegistrarLecturaConsumo(l);
                                clienteSocket.CerrarConexion();
                            }
                        }
                        else if (tipo == "trafico")
                        {
                            Lectura l = new Lectura()
                            {
                                NroSerie = medidor,
                                Fecha = fechaNew,
                                Tipo = tipo,
                                Valor = valor
                            };

                            lock (dal)
                            {
                                clienteSocket.Escribir(medidorV + "|OK");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine(medidorV + "|OK");
                                Console.ResetColor();
                                dal.RegistrarLecturaTrafico(l);
                                clienteSocket.CerrarConexion();
                            }
                        }
                    }
                }
                else
                {
                    clienteSocket.Escribir(DateTime.Now + "|" + medidorV + "|" + "ERROR");
                    Console.WriteLine(DateTime.Now + "|" + medidorV + "|" + "ERROR");
                    clienteSocket.CerrarConexion();
                }
                }
                else
                {
                clienteSocket.Escribir("Error en solicitud. Cerrando Conexión");
                Console.WriteLine(DateTime.Now + "|" + medidorV + "|" + "ERROR");
                clienteSocket.CerrarConexion();
            }
            }
        }
    }


