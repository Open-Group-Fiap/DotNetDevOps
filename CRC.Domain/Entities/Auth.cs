using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CRC.Domain.Entities;

[Table("T_OP_CRC_AUTH")]
[Index(nameof(Email), IsUnique = true)]
public class Auth
{
    [Key]
    [Column("ID_AUTH")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Column("EMAIL")]
    public string Email { get; set; }
    
    [Column("HASH_SENHA")]
    public string HashSenha { get; set; }
    
    [NotMapped]
    public Morador Morador { get; set; }
    
}