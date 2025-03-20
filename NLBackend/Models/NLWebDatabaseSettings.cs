namespace NLBackend.Models
{
    public class NLWebDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string NLCollectionName { get; set; } = null!;
    }
}
