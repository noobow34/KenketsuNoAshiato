using KenketsuNoAshiato.EF;

namespace KenketsuNoAshiato.Models
{
    public class UserModel
    {
        public required string UserId { get; set; }
        public KenketsuRoom[] Rooms { get; set; } = [];
    }
}
