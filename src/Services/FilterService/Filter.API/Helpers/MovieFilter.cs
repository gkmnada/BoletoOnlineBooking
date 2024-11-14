using System.Text.Json.Serialization;

namespace Filter.API.Helpers
{
    public class MovieFilter
    {
        [JsonPropertyName("movieName")]
        public string MovieName { get; set; }
        [JsonPropertyName("genre")]
        public string Genre { get; set; }
        [JsonPropertyName("language")]
        public string Language { get; set; }
    }
}
