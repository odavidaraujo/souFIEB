using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Soufieb.Webapp.Models
{
    [Table("TB_Aluno")]
    public class User
    {
        [Display(Name = "Nome")]
        [Column("NOME")]
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100)]
        public string? Nome { get; set; }

        [Display(Name = "CPF")]
        [Column("CPF")]
        [Required(ErrorMessage = "O campo CPF é obrigatório.")]
        [StringLength(11)]
        public string? CPF { get; set; }

        [Display(Name = "RG")]
        [Column("RG")]
        [Required(ErrorMessage = "O campo RG é obrigatório.")]
        [StringLength(10)]
        public string? RG { get; set; }

        [Display(Name = "Data de Nascimento")]
        [Column("DATANASC")]
        [Required(ErrorMessage = "O campo Data de Nascimento é obrigatório.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DataNascimento { get; set; }

        public string DataNascimentoFormatada
        {
            get
            {
                return DataNascimento?.ToString("dd/MM/yyyy");
            }
        }
        public string DataInput
        {
            get
            {
                return DataNascimento?.ToString("yyyy-MM-dd");
            }
        }


        [Display(Name = "Telefone")]
        [Column("TELEFONE")]
        [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
        [StringLength(20)]
        public string? Telefone { get; set; }

        [Display(Name = "Email")]
        [Column("EMAIL")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Display(Name = "Rua")]
        [Column("RUA")]
        [Required(ErrorMessage = "O campo Rua é obrigatório.")]
        [StringLength(100)]
        public string? Rua { get; set; }

        [Display(Name = "Numero_Rua")]
        [Column("NUM")]
        [Required(ErrorMessage = "O campo Número é obrigatório.")]
        [StringLength(10)]
        public string? Num { get; internal set; }

        [Display(Name = "CEP")]
        [Column("CEP")]
        [Required(ErrorMessage = "O campo CEP é obrigatório.")]
        [StringLength(8)]
        public string? CEP { get; set; }

        [Display(Name = "Bairro")]
        [Column("BAIRRO")]
        [Required(ErrorMessage = "O campo Bairro é obrigatório.")]
        [StringLength(50)]
        public string? Bairro { get; set; }

        [Display(Name = "Cidade")]
        [Column("CIDADE")]
        [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
        [StringLength(50)]
        public string? Cidade { get; set; }

        [Display(Name = "UF")]
        [Column("UF")]
        [Required(ErrorMessage = "O campo UF é obrigatório.")]
        [StringLength(2)]
        public string? UF { get; set; }

        [Display(Name = "Complemento")]
        [Column("COMPLEMENTO")]
        [StringLength(50)]
        public string? Complemento { get; set; }

        [Display(Name = "Senha")]
        [Column("SENHA")]
        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [StringLength(50)]
        public string? Senha { get; set; }

        [Display(Name = "Foto")]
        [Column("FOTO")]
        [Required(ErrorMessage = "O campo Foto é obrigatório.")]
        [StringLength(200)]
        public string? Foto { get; set; }

        [Display(Name = "Código QR")]
        [Column("CODQR")]
        [StringLength(13)]
        public string? CodigoQR { get; set; }

        [Display(Name = "Chave")]
        [Column("CHAVE")]
        [Required(ErrorMessage = "O campo Chave é obrigatório.")]
        [StringLength(18)]
        public string? Chave { get; set; }

        [Key]
        [Display(Name = "RM")]
        [Column("RM")]
        [Required(ErrorMessage = "O campo RM é obrigatório.")]
        [StringLength(5)]
        public string? RM { get; set; }

        [Display(Name = "Admin")]
        [Column("ADMIN")]
        [Required(ErrorMessage = "O campo Admin é obrigatório.")]
        [StringLength(1)]
        public string? Admin { get; set; }

        [Display(Name = "Limite Diário de Refeição")]
        [Column("LIMITE_DIARIO_REFEICAO")]
        public int? LimiteDiarioRefeicao { get; set; }

        [Display(Name = "Vezes Passadas")]
        [Column("VEZES_PASSADAS")]
        public int? VezesPassadas { get; set; }

        [Display(Name = "ID do Curso")]
        [Column("ID_CURSO")]
        [Required(ErrorMessage = "O campo ID do Curso é obrigatório.")]
        public int? IdCurso { get; set; }

        [Display(Name = "ID da Turma")]
        [Column("ID_TURMA")]
        [Required(ErrorMessage = "O campo ID da Turma é obrigatório.")]
        public int? IdTurma { get; set; }

        [Display(Name = "ID da Unidade")]
        [Column("ID_UNIDADE")]
        [Required(ErrorMessage = "O campo ID da Unidade é obrigatório.")]
        public int? IdUnidade { get; set; }

        [Display(Name = "Ativo ou Inativo")]
        [Column("ATIVO")]
        [Required(ErrorMessage = "O campo Ativo é obrigatório.")]
        public string? Ativo { get; set; }

        [Display(Name = "Data Criação")]
        [Column("DATA_CRIACAO")]
        [Required(ErrorMessage = "O campo Data de Criação é obrigatório.")]
        public DateTime? DataCriacao { get; set; }
    }
}
