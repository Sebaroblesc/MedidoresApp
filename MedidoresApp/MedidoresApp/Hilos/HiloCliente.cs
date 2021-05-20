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

            clienteSocket.Escribir("fecha|nromedidor|tipo");
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
                catch (Exception)
                {

                    input = "";
                }
                
                string[] frase = input.Split('|');
                if (frase.Length == 6)
                {
                    List<string> errores = new List<string>();

                    int medidor = Int32.Parse((input.Split('|'))[0]);
                    if (medidorV != medidor)
                    {
                        errores.Add("Codigo medidor no coincide.");
                    }

                    string fecha = (input.Split('|'))[1];
                    if (fecha != fechaV)
                    {
                        errores.Add("Fecha no coincide.");
                    }

                    string tipo = (input.Split('|'))[2];
                    if (tipo != "consumo" && tipo != "trafico")
                    {
                        errores.Add("Debe ser tipo consumo o trafico.");
                    }

                    int valor = Int32.Parse((input.Split('|'))[3]);
                    if (valor < 0 && valor > 1000)
                    {
                        errores.Add("Valor debe ser entre 0 y 1000.");
                    }
                    int estado = Int32.Parse((input.Split('|'))[4]);

                    if (estado > -1 && estado > 2)
                    {
                        errores.Add("Estado debe ser entre -1 y 2");
                    }

                    string confirmar = (input.Split('|'))[5];
                    if (confirmar != "UPDATE")
                    {
                        errores.Add("Debe ingresar UPDATE para confirmar.");
                    }

                    if (errores.Count > 0)
                    {
                        foreach (var error in errores)
                        {
                            clienteSocket.Escribir(error);
                            //clienteSocket.CerrarConexion();
                        }
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
                                clienteSocket.Escribir("Lectura de Consumo registrada con exito.");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("Lectura de consumo registrada con exito.");
                                Console.ResetColor();
                                dal.RegistrarLecturaConsumo(l);
                                //clienteSocket.CerrarConexion();
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

                                clienteSocket.Escribir("Lectura de trafico registrada con exito.");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("Lectura de trafico registrada con exito.");
                                Console.ResetColor();
                                dal.RegistrarLecturaTrafico(l);
                                //clienteSocket.CerrarConexion();
                            }
                        }
                    }
                }
                else if (frase.Length == 5)
                {
                    List<string> errores = new List<string>();
                    int medidor = Int32.Parse((input.Split('|'))[0]);
                    if (medidorV != medidor)
                    {
                        errores.Add("Codigo medidor no coincide.");
                    }

                    string fecha = (input.Split('|'))[1];
                    if (fecha != fechaV)
                    {
                        errores.Add("Fecha no coincide.");
                    }

                    string tipo = (input.Split('|'))[2];
                    if (tipo != "consumo" && tipo != "trafico")
                    {
                        errores.Add("Debe ser tipo consumo o trafico.");
                    }

                    int valor = Int32.Parse((input.Split('|'))[3]);
                    if (valor < 0 && valor > 1000)
                    {
                        errores.Add("Valor debe ser entre 0 y 1000.");
                    }

                    string confirmar = (input.Split('|'))[4];
                    if (confirmar != "UPDATE")
                    {
                        errores.Add("Debe ingresar UPDATE para confirmar.");
                    }

                    if (errores.Count > 0)
                    {
                        foreach (var error in errores)
                        {
                            clienteSocket.Escribir(error);
                            //clienteSocket.CerrarConexion();
                        }
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
                                clienteSocket.Escribir("Lectura de Consumo registrada con exito.");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("Lectura de consumo registrada con exito.");
                                dal.RegistrarLecturaConsumo(l);
                                //clienteSocket.CerrarConexion();
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
                            };

                            lock (dal)
                            {
                                clienteSocket.Escribir("Lectura de trafico registrada con exito.");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("Lectura de trafico registrada con exito.");
                                Console.ResetColor();
                                dal.RegistrarLecturaTrafico(l);
                                //clienteSocket.CerrarConexion();
                            }
                        }
                    }
                }
                else
                {
                    clienteSocket.Escribir(DateTime.UtcNow + "|" + medidorV + "|" + "ERROR");
                    //clienteSocket.CerrarConexion();
                }
                }
                else
                {                
                clienteSocket.Escribir("Error en solicitud. Cerrando Conexión");
                    //clienteSocket.CerrarConexion();
                }
            }
        }
    }


