using KenketsuNoAshiato.EF;

namespace KenketsuNoAshiato.Models
{
    public class UserModel
    {
        public required User User { get; set; }
        public KenketsuRoom[] Rooms { get; set; } = [];
    }
}
