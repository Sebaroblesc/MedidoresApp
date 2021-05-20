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
    
        public int NroSerie { get; set; }        
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public int Valor { get; set; }
        public int Estado { get; set; }

    }
    
}


