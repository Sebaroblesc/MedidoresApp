using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedidoresModel.DAL
{
    public class MedidorTraficoDALFactory
    {
        public static IMedidorTraficoDAL CreateDal()
        {
            return MedidorTraficoDALArchivos.GetInstancia();
        }
    }
}
