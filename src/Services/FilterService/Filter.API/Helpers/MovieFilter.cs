using System.Text.Json.Serialization;

namespace Filter.API.Helpers
{
    public class MovieFilter
    {
        [JsonPropertyName("movieName")]
        public string MovieName { get; set; }
        [JsonPropertyName("genre")]
        public List<string> Genre { get; set; }
        [JsonPropertyName("language")]
        public List<string> Language { get; set; }
    }
}
