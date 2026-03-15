using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Crane> Cranes => Set<Crane>();
    public DbSet<ConstructionSite> ConstructionSites => Set<ConstructionSite>();
    public DbSet<OperatorAssignment> OperatorAssignments => Set<OperatorAssignment>();
    public DbSet<WorkPlan> WorkPlans => Set<WorkPlan>();
    public DbSet<ProgressPayment> ProgressPayments => Set<ProgressPayment>();
    public DbSet<DailyWageRecord> DailyWageRecords => Set<DailyWageRecord>();
    public DbSet<OvertimeRecord> OvertimeRecords => Set<OvertimeRecord>();
    public DbSet<IncomeExpense> IncomeExpenses => Set<IncomeExpense>();
    public DbSet<FuelRecord> FuelRecords => Set<FuelRecord>();
    public DbSet<MaintenanceRecord> MaintenanceRecords => Set<MaintenanceRecord>();
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();
    public DbSet<News> News => Set<News>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Slider> Sliders => Set<Slider>();
    public DbSet<GalleryItem> GalleryItems => Set<GalleryItem>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<MenuRole> MenuRoles => Set<MenuRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
