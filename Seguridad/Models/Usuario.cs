using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seguridad.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        public string Nombre { get; set;}

        public string Apellido { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public byte Status { get; set; }
        
        public string ConfirmPassword { get; set; }
    }
}
