using dentist_panel_api.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dentist_panel_api
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var insertedEntries = this.ChangeTracker.Entries()
                                   .Where(x => x.State == EntityState.Added)
                                   .Select(x => x.Entity);
            foreach (var insertedEntry in insertedEntries)
            {
                var auditableEntity = insertedEntry as AuditableEntity;
                if (auditableEntity != null)
                {
                    auditableEntity.CreatedAt = DateTime.UtcNow;
                }
            }

            var modifiedEntries = this.ChangeTracker.Entries()
                       .Where(x => x.State == EntityState.Modified)
                       .Select(x => x.Entity);

            foreach (var modifiedEntry in modifiedEntries)
            {
                var auditableEntity = modifiedEntry as AuditableEntity;
                if (auditableEntity != null)
                {
                    auditableEntity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<HealthCoverage> HealthCoverages { get; set; }
        public DbSet<Odontogram> Odontograms { get; set; }
        public DbSet<Consult> Consults { get; set; }
        public DbSet<OdontogramProsthesis> OdontogramProstheses { get; set; }
        public DbSet<Decay> Decays { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<TipoDeTratamiento> TiposDeTratamiento { get; set; }
    }
}
