using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Soufieb.Webapp.Models
{
    [Table("TB_REFEICAO")]
    public class Statistics
    {
        [Key]
        [Column("COD_REFEICAO")]
        public int CodigoRefeicao { get; set; }

        [Column("DATA")]
        public DateTime Data { get; set; }


        [Column("PERIODO")]
        public string Periodo { get; set; }

        [Column("ID_ALUNO")]
        public string IdAluno { get; set; }

        [Column("ID_FUNCIONARIO")]
        public string? IdFuncionario { get; set; }

        [Column("ID_UNIDADE")]
        public int IdUnidade { get; set; }
    }
}
