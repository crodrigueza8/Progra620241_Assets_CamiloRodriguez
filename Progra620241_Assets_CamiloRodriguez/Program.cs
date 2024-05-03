using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Progra620241_Assets_CamiloRodriguez.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        //Aca agregamos los valores de conexion a la base de datos para nuestra aplicacion 

        //1. Agregar la info en una etiqueta en el archivo appsettings.json

        var CnnStrBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("CNNSTR"));

        //Because of security is better to add password info into this code or add external storage.
        //Investigate about User Secrets.

        CnnStrBuilder.Password = "progra6";

        //Now that we have extract the info from string connection, create another string to save it.

        string CnnStr = CnnStrBuilder.ConnectionString;

        //Now we connect our project to the database.//

        builder.Services.AddDbContext<Progra620241Context>(options => options.UseSqlServer(CnnStr));

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

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}