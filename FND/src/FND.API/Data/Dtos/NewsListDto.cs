using FND.API.Entities;

namespace FND.API.Data.Dtos
{
    public class ListNewsDto
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public int? Publisher_id { get; set; }
        public string Classification_Decision { get; set; }
        public DateTime CreatedOn { get; set; }

        public string? Comment { get; set; }
        public DateTimeOffset? ReviewCreatedOn { get; set; }
        public int? Status { get; set; }
        public string? ReviewFeedback { get; set; }
        public int? ReviewedBy { get; set; }
        public string? ReviewerName { get; set; }
        public string? Result { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

    }
}
