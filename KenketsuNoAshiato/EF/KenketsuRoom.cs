using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KenketsuNoAshiato.EF;

[Table("kenketsu_room", Schema = "ashiato")]
public partial class KenketsuRoom
{
    [Key]
    [Column("room_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RoomId { get; set; }

    [ForeignKey("Pref")]
    [Column("pref")]
    public int PrefId { get; set; }
    public Pref? Pref { get; set; }

    [Column("room_name")]
    [StringLength(100)]
    public string RoomName { get; set; } = null!;

    [Column("display_order")]
    public int? DisplayOrder { get; set; }

    [Column("image_path")]
    [StringLength(100)]
    public string? ImagePath { get; set; }
}
