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

        return services;
    }
}
