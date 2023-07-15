namespace FND.API.Entities
{
    public class News
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public int? Publisher_id { get; set; }
        public string Classification_Decision { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
