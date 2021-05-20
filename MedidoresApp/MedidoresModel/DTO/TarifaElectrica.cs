using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedidoresModel.DTO
{
    public class TarifaElectrica
    {
        private string codigo;
        private int factor;

        public string Codigo { get => codigo; set => codigo = value; }
        public int Factor { get => factor; set => factor = value; }
    }
}
