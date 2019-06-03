using Microsoft.AspNetCore.Mvc;
using MVC5RealWorld.Util.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC5RealWorld.Models.DB
{
    
    public class Article
    {
        
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 5)]
        [Remote("IsUserAvailable", "Article", ErrorMessage = "Titulo ja existe")]        
        public string Titulo { get; set; }
                
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@" + "[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$")]
        [EmailAddress]
        public string Email { get; set; }

        [Range(1, 10)]
        [Display(Name = "Numero de Autores")]
        public int NumberOfAuthors { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime? CreateDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0,9999999999999999.99, ConvertValueInInvariantCulture = true)]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Price { get; set; }
        
        [Display(Name = "NomeCampo", ResourceType = typeof(ResourceTeste))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "ErroMensagem", ErrorMessageResourceType = typeof(ResourceTeste))]
        public string Editora { get; set; }


        [Required(ErrorMessage = "The Password field is required.")]
        [StringLength(8, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
