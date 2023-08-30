using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FND.API.Entities
{
    public class Publication
    {
        [ForeignKey("Users")]
        [Key]
        public int Publication_Id { get; set; }
        public string Publication_Name { get; set; }
        public string RSS_Url { get; set; }
        public string NewsDiv { get; set; }
        public string LastFetchedNewsUrl { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public virtual Users Users { get; set;}
    }
}
