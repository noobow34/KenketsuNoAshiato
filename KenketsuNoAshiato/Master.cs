using KenketsuNoAshiato.EF;

namespace KenketsuNoAshiato
{
    public static class Master
    {
        public static CenterBlock[] CenterBlocks = [];
        public static Pref[] Prefectures = [];
        public static KenketsuRoom[] KenketsuRooms = [];

        public static void Load()
        {
            AshiatoContext dbContext = new();
            CenterBlocks = dbContext.CenterBlocks.OrderBy(cb => cb.DisplayOrder).ToArray();
            Prefectures = dbContext.Prefs.OrderBy(p => p.DisplayOrder).ToArray();
            KenketsuRooms = dbContext.KenketsuRooms.OrderBy(r => r.DisplayOrder).ToArray();
        }
    }
}
