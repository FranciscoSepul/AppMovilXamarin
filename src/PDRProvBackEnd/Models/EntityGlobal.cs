using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PDRProvBackEnd.Models
{
    public class EntityGlobal : IEntityGlobal
    {
        [Key]
        [Display(Name="Identificador")]
        public Guid Id { get; set; }        
        [Display(Name="Deshabilitado")]
        public bool Disabled {get; set;}

        [Display(Name="Expira")]
        public DateTime? Expires {get; set;}

        [Display(Name="Registro creado")]
        public DateTime CreatedAt { get; set; }
         [Display(Name="Registro editado")]
        public DateTime? UpdatedAt { get; set; }
        [Display(Name = "Comentario del registro")]
        [StringLength(2000)]
        public string Comments { get; set; }
    }
}