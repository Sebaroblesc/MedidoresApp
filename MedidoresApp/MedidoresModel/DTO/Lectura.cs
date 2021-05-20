using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedidoresModel.DTO
{
    public class Lectura
    {
        //private int nroSerie;
        //private DateTime fecha;
        //private string tipo;
        //private int valor;
        //private int estado;

        [JsonProperty(PropertyName = "NroSerie")]    
        public int NroSerie { get; set; }
        [JsonProperty(PropertyName = "Fecha")]
        public DateTime Fecha { get; set; }
        [JsonProperty(PropertyName = "Tipo")]
        public string Tipo { get; set; }
        [JsonProperty(PropertyName = "Valor")]
        public int Valor { get; set; }
        [JsonProperty(PropertyName = "Estado")]
        public int Estado { get; set; }

    }

    //public int NroSerie { get => nroSerie; set => nroSerie = value; }
    //public DateTime Fecha { get => fecha; set => fecha = value; }
    //public string Tipo { get => tipo; set => tipo = value; }
    //public int Valor { get => valor; set => valor = value; }
    //public int Estado { get => estado; set => estado = value; }

    //public override string ToString()
    //{
    //    //return "{\"Nro Serie\":" + NroSerie + ",\"Fecha\":\"" + Fecha + ", \"Tipo\":\"" + Tipo + ",\"Valor\":\"" + Valor + ",\"Estado\":\"" + Estado + "\"}";
    //    //return  Fecha + "|" + unidadMedida + "|"  + valor + "|" + tipo;
    //}
}


