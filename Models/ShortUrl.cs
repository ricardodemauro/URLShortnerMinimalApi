using Postgrest.Attributes;
using Supabase;

namespace URLShortnerMinimalApi.Models
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [Table("ShortUrls")]
    public class ShortUrl : SupabaseModel
    {
        [Column("url")]
        public string Url { get; set; }

        [Column("chunck")]
        public string Chunck { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("sub")]
        public string UserSub { get; set; }

        [Column("sub_display")]
        public string UserDisplay { get; set; }

        [Column("sub_email")]
        public string UserEmail { get; set; }

        [Column("sub_url")]
        public string UserUrl { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
