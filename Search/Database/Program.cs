using Database.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// CORS skal slås til i app'en. Ellers kan man ikke hente data fra et andet domæne.
var AllowCORS = "_AllowCORS";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowCORS, builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<DatabaseService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(AllowCORS);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
