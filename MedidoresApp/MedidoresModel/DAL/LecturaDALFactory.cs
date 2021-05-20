using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedidoresModel.DAL
{
    public class LecturaDALFactory
    {
        public static ILecturaDAL CreateDal()
        {
            return LecturaDALArchivos.GetInstancia();
        }
    }
}
