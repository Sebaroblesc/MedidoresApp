using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedidoresModel.DAL
{
    public class MedidorTraficoDALArchivos : IMedidorTraficoDAL
    {
        private MedidorTraficoDALArchivos()
        {

        }
        private static IMedidorTraficoDAL instancia;

        public static IMedidorTraficoDAL GetInstancia()
        {
            if (instancia == null)
            {
                instancia = new MedidorTraficoDALArchivos();
            }
            return instancia;
        }

        private static List<int> medidorTrafico = new List<int>()
        {
            6666,7777,8888,9999,0000
        };

        public List<int> ObtenerMedidores()
        {
            return medidorTrafico;
        }



    }
}
