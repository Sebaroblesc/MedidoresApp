using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedidoresModel.DTO
{
    public class ZonaHoraria
    {
        private string codigo;
        private string nombreLargo;

        public string Codigo { get => codigo; set => codigo = value; }
        public string NombreLargo { get => nombreLargo; set => nombreLargo = value; }
    }
}
