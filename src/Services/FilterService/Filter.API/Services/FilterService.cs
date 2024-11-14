using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Filter.API.Helpers;
using Filter.API.Models;

namespace Filter.API.Services
{
    public class FilterService : IFilterService
    {
        private readonly ElasticsearchClient _elasticClient;
        private string _indexName;

        public FilterService(ElasticsearchClient elasticClient, IConfiguration configuration)
        {
            _elasticClient = elasticClient;
            _indexName = configuration["Elasticsearch:Index"] ?? "";
        }

        public async Task<List<Movie>> MovieFilterAsync(MovieFilter movieFilter)
        {
            var queries = new List<Query>();

            if (!string.IsNullOrEmpty(movieFilter.MovieName))
            {
                queries.Add(new MatchQuery(new Field("movieName")) { Query = movieFilter.MovieName });
            }

            if (!string.IsNullOrEmpty(movieFilter.Genre))
            {
                queries.Add(new MatchQuery(new Field("genre")) { Query = movieFilter.Genre });
            }

            if (!string.IsNullOrEmpty(movieFilter.Language))
            {
                queries.Add(new MatchQuery(new Field("language")) { Query = movieFilter.Language });
            }

            var response = await _elasticClient.SearchAsync<Movie>
                (x => x.Index(_indexName).Query(x => x.Bool(x => x.Must(queries.ToArray()))));

            if (!response.IsValidResponse)
            {
                throw new Exception("An error occurred while filtering movies");
            }

            return response.Documents.ToList();
        }
    }
}
