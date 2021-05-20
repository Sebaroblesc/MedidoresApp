using MedidoresModel.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedidoresModel.DAL
{
    public interface ILecturaDAL
    {
        void RegistrarLecturaTrafico(Lectura l);
        void RegistrarLecturaConsumo(Lectura l);
        List<Lectura> GetLecturas();
    }
}
