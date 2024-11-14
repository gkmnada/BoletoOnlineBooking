namespace Search.API.Settings
{
    public interface IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string MovieCollectionName { get; set; }
    }
}
