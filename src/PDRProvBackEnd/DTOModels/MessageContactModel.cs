using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDRProvBackEnd.DTOModels
{
    public class MessageContactModel
    {
        [Display(Name = "Titulo")]
        [StringLength(256)]
        public string Id { get; set; }
        [Display(Name = "Titulo")]
        [StringLength(256)]
        public string Title { get; set; }

        [Display(Name = "Comentario")]
        public string Body { get; set; }

        [Display(Name = "Tipo de contacto")]
        public int TypeContact { get; set; }
        public Boolean IsRead { get; set; }
    }
}
