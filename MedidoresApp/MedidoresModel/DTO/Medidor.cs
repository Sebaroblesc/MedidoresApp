using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedidoresModel.DTO
{
    public class Medidor
    {
        // private int id;
        private DateTime fechaInstalacion;
        private int nroMedidorConsumo;
        private int nroMedidorTrafico;

        // public int Id { get => id; set => id = value; }
        public DateTime FechaInstalacion { get => fechaInstalacion; set => fechaInstalacion = value; }
        public int NroMedidorConsumo { get => nroMedidorConsumo; set => nroMedidorConsumo = value; }
        public int NroMedidorTrafico { get => nroMedidorTrafico; set => nroMedidorTrafico = value; }
    }
}
