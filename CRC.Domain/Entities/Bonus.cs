using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRC.Domain.Entities;

[Table("T_OP_CRC_BONUS")]
public class Bonus
{
    [Key]
    [Column("ID_BONUS")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Column("ID_CONDOMINIO")]
    public int IdCondominio { get; set; }
    [ForeignKey("IdCondominio")]
    public Condominio Condominio { get; set; }
    
    [Column("NOME")]
    public string Nome { get; set; }
    
    [Column("DESCRICAO")]
    public string? Descricao { get; set; }
    
    [Column("CUSTO")]
    public decimal Custo { get; set; }
    
    [Column("QTD_MAX")]
    public int QtdMax { get; set; }
    
    [NotMapped]
    public IEnumerable<MoradorBonus> MoradorBonus { get; set; }
    
    [NotMapped]
    public int Qtd { get; set; }
}