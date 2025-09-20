using dentist_panel_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace dentist_panel_api
{
    public class InsertFacialTreatment
    {
        public static async Task Main(string[] args)
        {
            var connectionString = "Server=82.180.174.52;Port=3306;Database=u651203601_estetica;Uid=u651203601_estetica;Pwd=Estetica1234#;SslMode=none;";
            
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            
            using var context = new ApplicationDbContext(optionsBuilder.Options);
            
            var facialTreatment = new TipoDeTratamiento
            {
                Id = Guid.NewGuid(),
                Nombre = "Facial",
                Descripcion = "Tratamiento facial para mejorar la apariencia y salud de la piel",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            context.TiposDeTratamiento.Add(facialTreatment);
            await context.SaveChangesAsync();
            
            Console.WriteLine("Tratamiento 'Facial' insertado exitosamente!");
        }
    }
}
