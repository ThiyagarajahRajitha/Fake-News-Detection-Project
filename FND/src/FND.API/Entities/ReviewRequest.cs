using System.ComponentModel.DataAnnotations.Schema;

namespace FND.API.Entities
{
    public class ReviewRequest
    {
        [ForeignKey("News")]
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Status { get; set; }
        public string? ReviewFeedback { get; set; }
        public string? Result { get; set; }

        [ForeignKey("Users")]
        public int ReviewedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public virtual Users Users { get; set; }
        public virtual News News { get; set; }
    }
}
