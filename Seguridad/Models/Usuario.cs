using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seguridad.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage ="El nombre de usuario es requerido")]
        public string Nombre { get; set;}

        [Required(ErrorMessage = "El apellido de usuario es requerido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El email de usuario es requerido")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "El estatus es requerido")]
        public byte Status { get; set; }

        [NotMapped]//esta prpiedad no sera mapeada en la base de datos.
        [Compare("Password", ErrorMessage ="La contraseña no coincide")]
        public string ConfirmPassword { get; set; }
    }
}
