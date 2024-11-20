using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRC.Domain.Entities;

[Table("T_OP_CRC_FATURA")]
public class Fatura
{
    [Key]
    [Column("ID_FATURA")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Column("ID_MORADOR")]
    public int IdMorador { get; set; }
    
    [ForeignKey("IdMorador")]
    public Morador Morador { get; set; }
    
    [Column("QTD_CONSUMIDA")]
    public int QtdConsumida { get; set; }
    
    [Column("DT_GERACAO")]
    public DateTime DtGeracao { get; set; }
    
    
    
}