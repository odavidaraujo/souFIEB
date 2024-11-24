using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Soufieb.Webapp.Models
{
    [Table("TB_CARDAPIO")]
    public class Cardapio
    {
        [Key]
        [Column("COD_CARDAPIO")]
        public int? CodCardapio { get; set; }

        [Column("DATA")]
        [Required(ErrorMessage = "O campo Data é obrigatório.")]
        public DateTime? Data { get; set; }

        [Display(Name = "Café da Manhã")]
        [Column("CAFE_MANHA")]
        [Required(ErrorMessage = "O campo Café da Manhã é obrigatório.")]
        [StringLength(100)]
        public string? CafeManha { get; set; }

        [Display(Name = "Prato Principal")]
        [Column("PRATO_PRINCIPAL")]
        [Required(ErrorMessage = "O campo Prato Principal é obrigatório.")]
        [StringLength(100)]
        public string? PratoPrincipal { get; set; }

        [Display(Name = "Janta")]
        [Column("JANTA")]
        [Required(ErrorMessage = "O campo Janta é obrigatório.")]
        [StringLength(100)]
        public string? Janta { get; set; }

        [Display(Name = "ID da Unidade")]
        [Column("ID_UNIDADE")]
        [Required(ErrorMessage = "O campo ID da Unidade é obrigatório.")]
        public int? IdUnidade { get; set; }

        [Display(Name = "Ativo ou Inativo")]
        [Column("STATUS")]
        [Required(ErrorMessage = "O campo Status é obrigatório.")]
        public string? Status { get; set; }

        [Display(Name = "Admin")]
        [Column("ADM")]
        [Required(ErrorMessage = "O campo Admin é obrigatório.")]
        [StringLength(100)]
        public string? Admin { get; set; }
    }
}
