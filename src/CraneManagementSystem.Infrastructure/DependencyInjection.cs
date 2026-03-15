using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Application.Services;
using CraneManagementSystem.Infrastructure.Options;
using CraneManagementSystem.Infrastructure.Repositories;
using CraneManagementSystem.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CraneManagementSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Options
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICraneRepository, CraneRepository>();
        services.AddScoped<IConstructionSiteRepository, ConstructionSiteRepository>();
        services.AddScoped<IOperatorAssignmentRepository, OperatorAssignmentRepository>();
        services.AddScoped<IWorkPlanRepository, WorkPlanRepository>();
        services.AddScoped<IProgressPaymentRepository, ProgressPaymentRepository>();
        services.AddScoped<IDailyWageRecordRepository, DailyWageRecordRepository>();
        services.AddScoped<IOvertimeRecordRepository, OvertimeRecordRepository>();
        services.AddScoped<IIncomeExpenseRepository, IncomeExpenseRepository>();
        services.AddScoped<IFuelRecordRepository, FuelRecordRepository>();
        services.AddScoped<IMaintenanceRecordRepository, MaintenanceRecordRepository>();
        services.AddScoped<ISystemSettingRepository, SystemSettingRepository>();
        services.AddScoped<INewsRepository, NewsRepository>();
        services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
        services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        // Auth & Token & Email
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAuthService, AuthService>();

        // Application services
        services.AddScoped<UserService>();
        services.AddScoped<CompanyService>();
        services.AddScoped<CraneService>();
        services.AddScoped<ConstructionSiteService>();
        services.AddScoped<WorkPlanService>();
        services.AddScoped<ProgressPaymentService>();
        services.AddScoped<DailyWageService>();
        services.AddScoped<OvertimeService>();
        services.AddScoped<IncomeExpenseService>();
        services.AddScoped<FuelTrackingService>();
        services.AddScoped<MaintenanceService>();
        services.AddScoped<ReportService>();
        services.AddScoped<SystemSettingService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IHomePageService, HomePageService>();
        services.AddScoped<NewsService>();
        services.AddScoped<AnnouncementService>();
        services.AddScoped<ContactService>();
        services.AddScoped<AppointmentService>();

        return services;
    }
}
