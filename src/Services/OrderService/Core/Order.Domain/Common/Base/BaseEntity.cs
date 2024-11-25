namespace Order.Domain.Common.Base
{
    public class BaseEntity
    {
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public bool is_active { get; set; }

        public BaseEntity()
        {
            id = Guid.NewGuid().ToString("D");
            created_at = DateTime.UtcNow;
            is_active = true;
        }
    }
}
