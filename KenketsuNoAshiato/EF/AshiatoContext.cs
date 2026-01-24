using Microsoft.EntityFrameworkCore;

namespace KenketsuNoAshiato.EF;

public partial class AshiatoContext : DbContext
{
    public AshiatoContext() => AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    public AshiatoContext(DbContextOptions<AshiatoContext> options) : base(options) => AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    public virtual DbSet<CenterBlock> CenterBlocks { get; set; }

    public virtual DbSet<Pref> Prefs { get; set; }

    public virtual DbSet<KenketsuRoom> KenketsuRooms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VisitStamp> VisitStamps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = Environment.GetEnvironmentVariable("KENKETSUNOASHIATO_CONNECTION_STRING") ?? "";
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CenterBlock>(entity =>
        {
            entity.HasKey(e => e.CenterBlockId).HasName("center_block_pkey");
        });

        modelBuilder.Entity<KenketsuRoom>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("kenketsu_room_pkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");
        });

        modelBuilder.Entity<VisitStamp>(entity =>
        {
            entity.HasKey(e => e.StampId).HasName("visit_stamp_pkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
