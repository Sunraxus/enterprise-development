var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");

var postgresDb = postgres.AddDatabase("realestate-db");

builder.AddProject<Projects.EstateAgency_Api>("Api")
    .WithReference(postgresDb, "DefaultConnection")
    .WaitFor(postgresDb);

builder.AddProject<Projects.EstateAgency_Grpc_Client>("estateagency-grpc-client");

builder.Build().Run();