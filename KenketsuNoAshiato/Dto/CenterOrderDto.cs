namespace KenketsuNoAshiato.Dto
{
    public class CenterOrderDto
    {
        public int CenterBlockId { get; set; }
        public int DisplayOrder { get; set; }
        public List<PrefOrderDto>? Prefectures { get; set; }
    }
}
