using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PDRProvBackEnd.DTOModels
{
    public class FilterModel
    {
        [Display(Name = "Propiedad")]
        [Required]
        public string Prop { get; set; }
        [Display(Name = "Tipo de condición")]
        [Required]
        public Builders.Anotations.TypeFilter CondType { get; set; }
        [Display(Name = "Valor")]
        public object Value { get; set; }

        [Display(Name = "Operador")]
        public Builders.Anotations.LogicType? LogicType { get; set; }

        [Display(Name = "Siguiente condicional")]
        public FilterModel NextCond { get; set; }
    }
}
