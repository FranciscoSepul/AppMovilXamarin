using System;
using System.ComponentModel.DataAnnotations;

namespace PDRProvBackEnd.Models
{
    public interface IEntityGlobal
    {
        public Guid Id { get; set; }        
        public bool Disabled {get; set;}
        public DateTime? Expires {get; set;}
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Comments { get; set; }
    }
}
