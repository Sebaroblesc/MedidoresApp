using MedidoresModel.DAL;
using MedidoresModel.DTO;
using SocketUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedidoresApp
{
    public partial class Program
    {       
        private static ClienteSocket clienteSocket;
        private static ILecturaDAL dal = LecturaDALFactory.CreateDal();
        private static IMedidorConsumoDAL dalConsumo = MedidorConsumoDALFactory.CreateDal();       
        

        static bool Menu()
        {
            bool continuar = true;
            Console.WriteLine("Ingrese opcion del menu:");
            Console.WriteLine("1. Mostrar Lecturas de Trafico.");
            Console.WriteLine("2. Mostrar Lecturas de Consumo.");            
            string opcion = Console.ReadLine().Trim();
            switch (opcion)
            {
                case "1":
                    MostrarLecturasTrafico();
                    break;
                case "2":
                    MostrarLecturasConsumo();
                    break;
                case "0":
                    continuar = false;
                    break;
                default:
                    Console.WriteLine("Ingrese opcion Válida");
                    break;
            }
            return continuar;
        }

        private static void MostrarLecturasTrafico()
        {
            try
            {
                dal.ObtenerLecturasTrafico();
            }
            catch (Exception)
            {
                Console.WriteLine("No existen registros en el sistema.");
            }
           
        }

        private static void MostrarLecturasConsumo()
        {
            try
            {
                dal.ObtenerLecturasConsumo();
            }
            catch (Exception)
            {
                Console.WriteLine("No existen registros en el sistema.");
            }

        }

        private static void IngresarLectura()
        {
            bool verificado = false;

            clienteSocket.Escribir("fecha|nromedidor|tipo");
            string prueba = clienteSocket.Leer();
            string fechaV = (prueba.Split('|'))[0];
            int medidorV = Int32.Parse((prueba.Split('|'))[1]);
            string tipoV = (prueba.Split('|'))[2];

            DateTime fechaNew = DateTime.ParseExact(fechaV, "yyyy-MM-dd-HH-mm-ss",
            System.Globalization.CultureInfo.InvariantCulture);

            foreach (var medidor in dalConsumo.ObtenerMedidores())
            {
                if (medidorV == medidor)
                {
                    verificado = true;
                }
            }

            if (verificado == true && ((DateTime.UtcNow - fechaNew).Minutes < 30) && (tipoV == "trafico" || tipoV == "consumo"))
            {
                clienteSocket.Escribir(DateTime.Now + "| WAIT");
                clienteSocket.Escribir("Ingrese:");
                clienteSocket.Escribir("Número Medidor|Fecha|Tipo|Valor|Estado (opcional)|UPDATE");
                string input = clienteSocket.Leer();
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
                            clienteSocket.CerrarConexion();
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
                                clienteSocket.Escribir("Lectura de trafico registrada con exito.");
                                dal.RegistrarLecturaTrafico(l);
                                clienteSocket.CerrarConexion();
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
                            clienteSocket.CerrarConexion();
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
                            };
                            lock (dal)
                            {
                                clienteSocket.Escribir("Lectura de trafico registrada con exito.");
                                dal.RegistrarLecturaTrafico(l);
                                clienteSocket.CerrarConexion();
                            }
                        }
                    }
                }
                else
                {
                    clienteSocket.Escribir(DateTime.UtcNow + "|" + medidorV + "|" + "ERROR");
                }
                }
                else
                {
                    clienteSocket.Escribir("Error en solicitud. Cerrando Conexión");
                    clienteSocket.CerrarConexion();
                }
            }
        }
    }
