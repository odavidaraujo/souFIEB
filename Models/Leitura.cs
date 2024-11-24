using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Soufieb.Webapp.Models
{
    [Table("TB_LEITURAINFO")]
    public class Leitura
    {
        [Key]
        [Column("COD_LEITURA")]
        public int? CodLeitura { get; set; }

        [Column("DATA")]
        [Required(ErrorMessage = "O campo Data é obrigatório.")]
        public DateTime? Data { get; set; }

        [Display(Name = "ID do INFORMATIVO")]
        [Column("ID_INFORMATIVO")]
        [Required(ErrorMessage = "O campo ID do INFORMATIVO é obrigatório.")]
        public int? IdInformativo { get; set; }

        [Display(Name = "ID do USUARIO")]
        [Column("ID_USUARIO")]
        [Required(ErrorMessage = "O campo ID do USUARIO é obrigatório.")]
        public int? IdUsuario { get; set; }
    }
}
