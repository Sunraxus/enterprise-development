var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var postgresDb = postgres.AddDatabase("realestate-db");

var api = builder.AddProject<Projects.EstateAgency_Api>("Api")
    .WithReference(postgresDb, "DefaultConnection")
    .WaitFor(postgresDb);

builder.AddProject<Projects.EstateAgency_Grpc_Client>("estateagency-grpc-client")
    .WithEnvironment("Worker__ServerAddress", api.GetEndpoint("https"))
    .WaitFor(api);

builder.Build().Run();