using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRC.Domain.Entities;

[Table("T_OP_CRC_CONDOMINIO")]
public class Condominio
{
    [Key]
    [Column("ID_CONDOMINIO")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Column("NOME")]
    public string Nome { get; set; }
    
    [Column("ENDERECO")]
    public string Endereco { get; set; }
    
    [NotMapped]
    public IEnumerable<Morador> Moradores { get; set; }
    
    [NotMapped]
    public IEnumerable<Bonus> Bonus { get; set; }
}