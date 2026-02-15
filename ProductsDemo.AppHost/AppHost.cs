var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");

var productsDb = postgres.AddDatabase("productsdb");

var api = builder.AddProject<Projects.ProductsDemo_WebAPI>("webapi")
    .WithReference(productsDb)
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.ProductsDemo_UI>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();