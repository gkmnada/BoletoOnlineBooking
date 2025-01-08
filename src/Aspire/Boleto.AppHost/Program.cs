var builder = DistributedApplication.CreateBuilder(args);

var catalog = builder.AddProject<Projects.Catalog_API>("catalog-service");
var search = builder.AddProject<Projects.Search_API>("search-service");
var filter = builder.AddProject<Projects.Filter_API>("filter-service");
var identity = builder.AddProject<Projects.Identity_API>("identity-service");
var ticket = builder.AddProject<Projects.Ticket_API>("ticket-service");
var discount = builder.AddProject<Projects.Discount_API>("discount-service");
var booking = builder.AddProject<Projects.Booking_API>("booking-service");
var payment = builder.AddProject<Projects.Payment_API>("payment-service");
var order = builder.AddProject<Projects.Order_API>("order-service");
var notification = builder.AddProject<Projects.Notification_API>("notification-api");

var gateway = builder.AddProject<Projects.ApiGateway_Ocelot>("api-gateway");

gateway.WaitFor(catalog);
gateway.WaitFor(search);
gateway.WaitFor(filter);
gateway.WaitFor(identity);
gateway.WaitFor(ticket);
gateway.WaitFor(discount);
gateway.WaitFor(booking);
gateway.WaitFor(payment);
gateway.WaitFor(order);
gateway.WaitFor(notification);

var frontend = builder.AddProject<Projects.Boleto_WebUI>("boleto-webui");

frontend.WaitFor(gateway);

builder.Build().Run();
