using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PDRProvBackEnd.DTOModels
{
    public class UserModel
    {
        public string Id {get; set;}
        [Display(Name ="Username")]
        [StringLength(256)]
        public string Username { get; set; }
        
        [Display(Name ="Email")]
        [StringLength(256)]
        public string Email { get; set; }    

        [JsonIgnore]
        [Display(Name ="Password")]
        [StringLength(256)]
        public string Password { get; set; }  

        [Display(Name ="Token")]
        public string Token { get; set; }     

        [Display(Name ="Token Expira")]
        public DateTime? TokenExpires {get; set;}       
    }
}