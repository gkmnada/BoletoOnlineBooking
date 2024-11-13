namespace Catalog.Domain.Common.Base
{
    public class BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        public BaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }
}
