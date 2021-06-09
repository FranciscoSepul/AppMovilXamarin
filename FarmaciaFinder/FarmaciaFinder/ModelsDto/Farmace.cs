using System;
using System.Collections.Generic;
using System.Text;

namespace FarmaciaFinder.ModelsDto
{
    public class Farmace
    {
        public string fecha { get; set; }
        public string local_id { get; set; }
        public string fk_region { get; set; }
        public string fk_comuna { get; set; }
        public string fk_localidad { get; set; }
        public string local_nombre { get; set; }
        public string comuna_nombre { get; set; }
        public string localidad_nombre { get; set; }
        public string local_direccion { get; set; }
        public string funcionamiento_hora_apertura { get; set; }
        public string funcionamiento_hora_cierre { get; set; }
        public string local_telefono { get; set; }
        public string local_lat { get; set; }
        public string local_lng { get; set; }
        public string funcionamiento_dia { get; set; }
    }
}
