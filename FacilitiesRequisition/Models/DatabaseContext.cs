using FacilitiesRequisition.Models.Administrators;
using FacilitiesRequisition.Models.Officers;
using FacilitiesRequisition.Models.Faculties;
using Microsoft.EntityFrameworkCore;

namespace FacilitiesRequisition.Models; 

public class DatabaseContext : DbContext {
    public DbSet<Administrator> Administrators { get; set; }
    public DbSet<AdministratorRole> AdministratorRoles { get; set; }
    
    public DbSet<Officer> Officers { get; set; }
    public DbSet<OfficerRole> OfficerRoles { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<FacultyRole> FacultyRoles { get; set; }
    public DbSet<College> Colleges { get; set; }
    
    public DbSet<SchoolTag> SchoolTags { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlServer(
            @"Server=(localdb)\mssqllocaldb;Database=FacilitiesRequisitions;Trusted_Connection=True");
    }
}