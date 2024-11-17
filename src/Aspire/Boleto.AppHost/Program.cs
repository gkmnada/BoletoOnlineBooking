var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Catalog_API>("catalog-service");
builder.AddProject<Projects.Search_API>("search-service");
builder.AddProject<Projects.Filter_API>("filter-service");
builder.AddProject<Projects.Identity_API>("identity-service");

builder.Build().Run();