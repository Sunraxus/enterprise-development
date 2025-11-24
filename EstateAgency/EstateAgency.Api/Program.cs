using EstateAgency.Application.Mapper;
using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Interface;
using EstateAgency.Infrastructure.Persistence;
using EstateAgency.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AppMapper>());

builder.Services.AddScoped<IRepository<Counterparty>>(provider =>
    new Repository<Counterparty>(provider.GetRequiredService<AppDbContext>()));
builder.Services.AddScoped<IRepository<RealEstate>>(provider =>
    new Repository<RealEstate>(provider.GetRequiredService<AppDbContext>()));
builder.Services.AddScoped<IRepository<Application>>(provider =>
    new Repository<Application>(provider.GetRequiredService<AppDbContext>()));
 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
    foreach (var xmlFile in xmlFiles)
        c.IncludeXmlComments(xmlFile);
});

var app = builder.Build();

app.MapDefaultEndpoints();
 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    await DbSeeder.SeedAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();