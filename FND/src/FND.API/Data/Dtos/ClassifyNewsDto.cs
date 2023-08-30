namespace FND.API.Data.Dtos
{
    public class ClassifyNewsDto
    {
        public string? Topic { get; set; }
        public string? Url { get; set; }

        public int? Publication_Id { get; set; }
        public string Content { get; set; }
    }
}
