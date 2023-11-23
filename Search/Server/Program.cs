using Server.Service;
using Shared;

public class Program
{
    public static void Main(string[] args)
    {
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

        builder.Services.AddSingleton<SearchService>();
        builder.Services.AddSingleton<SearchSettings>();
        builder.Services.AddSingleton<CommandService>();

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
    }
}
