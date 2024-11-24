using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Soufieb.Webapp.Models
{
    [Table("TB_INFORMATIVO")]
    public class Informatives
    {
        [Key]
        [Column("COD_INFORMATIVO")]
        public int CodigoInformativo { get; set; }

        [Display(Name = "Admin")]
        [Column("ADM")]
        [Required(ErrorMessage = "O campo Admin é obrigatório.")]
        [StringLength(100)]
        public string Admin { get; set; }

        [Display(Name = "Título")]
        [Column("TITULO")]
        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        [StringLength(120)]
        public string Titulo { get; set; }

        [Display(Name = "Mensagem")]
        [Column("MENSAGEM")]
        [Required(ErrorMessage = "O campo Mensagem é obrigatório.")]
        public string Mensagem { get; set; }

        [Display(Name = "Data")]
        [Column("DATA")]
        [Required(ErrorMessage = "O campo Data é obrigatório.")]
        [StringLength(100)]
        public string Data { get; set; }

        [Display(Name = "Status")]
        [Column("STATUS")]
        [Required(ErrorMessage = "O campo Status é obrigatório.")]
        public int Status { get; set; }

        [Display(Name = "Checked")]
        [Column("CHECKED")]
        [Required(ErrorMessage = "O campo Checked é obrigatório.")]
        public int Checked { get; set; }

        [NotMapped]
        public int NumeroLeituras { get; set; }

        [NotMapped]
        public int NumeroTotalLeituras { get; set; }

        [Display(Name = "Data Criação")]
        [Column("DATA_CRIACAO")]
        [Required(ErrorMessage = "O campo Data de Criação é obrigatório.")]
        public DateTime? DataCriacao { get; set; }

    }
}
