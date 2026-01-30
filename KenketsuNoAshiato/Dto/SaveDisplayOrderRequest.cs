namespace KenketsuNoAshiato.Dto
{
    public class SaveDisplayOrderRequest
    {
        public string? UserId { get; set; }
        public List<CenterOrderDto>? Regions { get; set; }
    }
}
