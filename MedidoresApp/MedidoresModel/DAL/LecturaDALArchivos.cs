using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedidoresModel.DTO;
using Newtonsoft.Json;

namespace MedidoresModel.DAL
{
    public class LecturaDALArchivos : ILecturaDAL
    {
        private LecturaDALArchivos()
        {

        }

        private static ILecturaDAL instancia;

        public static ILecturaDAL GetInstancia()
        {
            if (instancia == null)
                instancia = new LecturaDALArchivos();
            return instancia;
        }


        private string archivoTrafico = Directory.GetCurrentDirectory()
            + Path.DirectorySeparatorChar + "traficos.txt";

        private string archivoConsumo = Directory.GetCurrentDirectory()
            + Path.DirectorySeparatorChar + "consumos.txt";                  


        public void RegistrarLecturaConsumo(Lectura l)
        {            
            try
            {
                List<Lectura> lecturas2 = JsonConvert.DeserializeObject<List<Lectura>>(File.ReadAllText(archivoConsumo));
                lecturas2.Add(l);
                string json = JsonConvert.SerializeObject(lecturas2, Formatting.Indented);
                File.WriteAllText(archivoConsumo, json);

            }
            catch (FileNotFoundException ex)
            {
                string json = JsonConvert.SerializeObject(l, Formatting.Indented);
                string json2 = "[" + json + "]";
                File.WriteAllText(archivoConsumo, json2);
            } 
        }


        public void RegistrarLecturaTrafico(Lectura l)
        {
            try
            {
                List<Lectura> lecturas2 = JsonConvert.DeserializeObject<List<Lectura>>(File.ReadAllText(archivoTrafico));
                lecturas2.Add(l);
                string json = JsonConvert.SerializeObject(lecturas2, Formatting.Indented);
                File.WriteAllText(archivoTrafico, json);

            }
            catch (FileNotFoundException ex)
            {
                string json = JsonConvert.SerializeObject(l, Formatting.Indented);
                string json2 = "[" + json + "]";
                File.WriteAllText(archivoTrafico, json2);
            }
        }

        public List<Lectura> ObtenerLecturasConsumo()
        {

            List<Lectura> lecturas2 = JsonConvert.DeserializeObject<List<Lectura>>(File.ReadAllText(archivoConsumo),
            new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects});

            foreach (var l in lecturas2)
            {
                Console.WriteLine("Numero de Serie: " + l.NroSerie);
                Console.WriteLine("Fecha: " + l.Fecha);
                Console.WriteLine("Tipo: " + l.Tipo);
                Console.WriteLine("Valor: " + l.Valor);
                Console.WriteLine("Estado: " + l.Estado);
                Console.WriteLine(" ");
            }
            return lecturas2;
        }

        public List<Lectura> ObtenerLecturasTrafico()
        {
         
                List<Lectura> lecturas2 = JsonConvert.DeserializeObject<List<Lectura>>(File.ReadAllText(archivoTrafico),
                new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

                foreach (var l in lecturas2)
                {
                    Console.WriteLine("Numero de Serie: " + l.NroSerie);
                    Console.WriteLine("Fecha: " + l.Fecha);
                    Console.WriteLine("Tipo: " + l.Tipo);
                    Console.WriteLine("Valor: " + l.Valor);
                    Console.WriteLine("Estado: " + l.Estado);
                    Console.WriteLine(" ");
                }
                return lecturas2;
            }            
        }
    }
