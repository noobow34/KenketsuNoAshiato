using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KenketsuNoAshiato.EF;

[Table("users", Schema = "ashiato")]
public partial class User
{
    [Key]
    [Column("user_id")]
    [StringLength(10)]
    public string UserId { get; set; } = null!;

    [Column("registered_at")]
    public DateTime? RegisteredAt { get; set; }

    [Column("last_access_at")]
    public DateTime? LastAccessAt { get; set; }
}
