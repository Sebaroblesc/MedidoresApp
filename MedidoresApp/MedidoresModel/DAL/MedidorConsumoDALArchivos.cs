using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedidoresModel.DTO;

namespace MedidoresModel.DAL
{
    public class MedidorConsumoDALArchivos : IMedidorConsumoDAL
    {
        private MedidorConsumoDALArchivos()
        {

        }
        private static IMedidorConsumoDAL instancia;

        public static IMedidorConsumoDAL GetInstancia()
        {
            if(instancia == null)
            {
                instancia = new MedidorConsumoDALArchivos();
            }
            return instancia;
        }

        private static List<int> medidorConsumo = new List<int>()
        {
            11,22,33,44,55
        };

        public List<int> ObtenerMedidores()
        {
            return medidorConsumo;
        }
               


    }
}
