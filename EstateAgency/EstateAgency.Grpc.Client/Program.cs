using EstateAgency.Grpc.Client;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<WorkerOptions>(
    builder.Configuration.GetSection(WorkerOptions.SectionName));

builder.Services.AddSingleton<ApplicationContractGenerator>();

builder.Services.AddHostedService<Worker>();

var app = builder.Build();
app.Run();