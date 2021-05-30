using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDRProvBackEnd.Models
{
    public class MessageContact:EntityGlobal
    {
        
        [Display(Name = "Titulo")]
        [StringLength(256)]
        public string Title { get; set; }

        [Display(Name = "Comentario")]
        public string Body { get; set; }

        [Display(Name = "Tipo de contacto")]
        public int TypeContact { get; set; }
        public Boolean IsRead { get; set; }
        public MessageContact ResponseTo { get; set; }
        public User SendingUser { get; set; }
        public ReasonContact ReasonContact { get; set; }

    }
}
