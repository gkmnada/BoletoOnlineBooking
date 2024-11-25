var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Catalog_API>("catalog-service");
builder.AddProject<Projects.Search_API>("search-service");
builder.AddProject<Projects.Filter_API>("filter-service");
builder.AddProject<Projects.Identity_API>("identity-service");
builder.AddProject<Projects.Ticket_API>("ticket-service");
builder.AddProject<Projects.Discount_API>("discount-service");
builder.AddProject<Projects.Booking_API>("booking-service");
builder.AddProject<Projects.Payment_API>("payment-service");
builder.AddProject<Projects.Order_API>("order-service");

builder.Build().Run();
