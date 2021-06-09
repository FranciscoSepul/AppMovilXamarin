using FarmaciaFinder.ModelsDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FarmaciaFinder
{
    public interface Services
    {
        Task<List<Farmace>> ListarFarmacia();
        List<Regiones> listarRegion();
        Task<List<Comuna>> listarComunas(int region);
    }
}
