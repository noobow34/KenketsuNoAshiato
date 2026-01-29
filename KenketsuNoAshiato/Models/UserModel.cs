using KenketsuNoAshiato.EF;

namespace KenketsuNoAshiato.Models
{
    public class UserModel
    {
        public required User User { get; set; }
        public KenketsuRoom[] Rooms { get; set; } = [];

        public CenterBlock[] CenterBlocks { get; set; } = [];

        public Pref[] Prefectures { get; set; } = [];

        public bool IsShare { get; set; }

        public string ShareId { get; set; } = string.Empty;
    }
}
