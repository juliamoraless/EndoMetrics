using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra;

public class EndoMetricsContext: DbContext
{
    public EndoMetricsContext(DbContextOptions<EndoMetricsContext> options) : base(options)
    { }  

    public DbSet<User> Users { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<SurgeryData> SurgeryDatas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Patient>()
            .HasOne(p => p.SurgeryData)          
            .WithOne(s => s.Patient)             
            .HasForeignKey<SurgeryData>(s => s.PatientId) 
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<SurgeryData>(entity =>
        {
            entity.ToTable("SurgeryDatas");
            entity.HasKey(s => s.Id);

            entity.OwnsOne(s => s.SurgicalFindings, findings =>
            {
                findings.Property(f => f.FimbriaScore)
                    .HasColumnName("SurgicalFindings_Fimbria")
                    .IsRequired();

                findings.Property(f => f.OvaryScore)
                    .HasColumnName("SurgicalFindings_Ovary")
                    .IsRequired();
            });

            entity.OwnsOne(s => s.AfsScore, afs =>
            {
                afs.Property(a => a.EndometriosisScore)
                    .HasColumnName("Afs_Endometriosis")
                    .IsRequired();

                afs.Property(a => a.TotalScore)
                    .HasColumnName("Afs_Total")
                    .IsRequired();
            });
        });
    }
}