using KenketsuNoAshiato;
using KenketsuNoAshiato.EF;

string connectionString = Environment.GetEnvironmentVariable("KENKETSUNOASHIATO_CONNECTION_STRING") ?? "";
Console.WriteLine($"KENKETSUNOASHIATO_CONNECTION_STRING:{connectionString?.Length ?? 0}");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<Microsoft.Extensions.WebEncoders.WebEncoderOptions>(options =>
{
    options.TextEncoderSettings = new System.Text.Encodings.Web.TextEncoderSettings(System.Text.Unicode.UnicodeRanges.All);
});
builder.Services.AddDbContext<AshiatoContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

Master.Load();

app.Run();
