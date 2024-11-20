using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRC.Domain.Entities;

[Table("T_OP_CRC_MORADOR_BONUS")]
public class MoradorBonus
{
    [Key]
    [Column("ID_MB")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Column("ID_MORADOR")]
    public int IdMorador { get; set; }
    [ForeignKey("IdMorador")]
    public Morador Morador { get; set; }
    
    [Column("ID_BONUS")]
    public int IdBonus { get; set; }
    [ForeignKey("IdBonus")]
    public Bonus Bonus { get; set; }
    
    [Column("QTD")]
    public int Qtd { get; set; }
    
}