using CraneManagementSystem.Application.DTOs.Dashboard;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain;

namespace CraneManagementSystem.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly ICraneRepository _craneRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IConstructionSiteRepository _constructionSiteRepository;
    private readonly IUserRepository _userRepository;
    private readonly IIncomeExpenseRepository _incomeExpenseRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IContactMessageRepository _contactMessageRepository;

    public DashboardService(
        ICraneRepository craneRepository,
        ICompanyRepository companyRepository,
        IConstructionSiteRepository constructionSiteRepository,
        IUserRepository userRepository,
        IIncomeExpenseRepository incomeExpenseRepository,
        IAppointmentRepository appointmentRepository,
        IContactMessageRepository contactMessageRepository)
    {
        _craneRepository = craneRepository;
        _companyRepository = companyRepository;
        _constructionSiteRepository = constructionSiteRepository;
        _userRepository = userRepository;
        _incomeExpenseRepository = incomeExpenseRepository;
        _appointmentRepository = appointmentRepository;
        _contactMessageRepository = contactMessageRepository;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync(Guid? companyId, CancellationToken cancellationToken = default)
    {
        var cranes = await _craneRepository.GetAllAsync(cancellationToken);
        var companies = await _companyRepository.GetAllAsync(cancellationToken);
        var sites = await _constructionSiteRepository.GetAllAsync(cancellationToken);
        var users = await _userRepository.GetAllAsync(cancellationToken);
        var incomeExpenses = await _incomeExpenseRepository.GetAllAsync(cancellationToken);
        var appointments = await _appointmentRepository.GetAllAsync(cancellationToken);
        var contactMessages = await _contactMessageRepository.GetAllAsync(cancellationToken);

        var operators = users.Where(u => u.Role == UserRole.Operatör).ToList();
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var incomeExpenseList = incomeExpenses.AsEnumerable();
        if (companyId.HasValue)
            incomeExpenseList = incomeExpenseList.Where(x => x.CompanyId == companyId);

        var incomeThisMonth = incomeExpenseList.Where(x => x.Date >= startOfMonth && x.Type == IncomeExpenseType.Gelir).Sum(x => x.Amount);
        var expenseThisMonth = incomeExpenseList.Where(x => x.Date >= startOfMonth && x.Type == IncomeExpenseType.Gider).Sum(x => x.Amount);

        var appointmentList = appointments.AsEnumerable();
        if (companyId.HasValue)
            appointmentList = appointmentList.Where(x => x.CompanyId == companyId);
        var pendingAppointments = appointmentList.Count(x => x.Status == "Beklemede" || string.IsNullOrEmpty(x.Status));

        var unreadMessages = contactMessages.Count(x => !x.IsRead);

        return new DashboardSummaryDto
        {
            TotalCranes = cranes.Count,
            ActiveCranes = cranes.Count(c => c.Status == "Aktif" || string.IsNullOrEmpty(c.Status)),
            TotalCompanies = companies.Count,
            TotalConstructionSites = sites.Count,
            TotalOperators = operators.Count,
            TotalIncomeThisMonth = incomeThisMonth,
            TotalExpenseThisMonth = expenseThisMonth,
            PendingAppointments = pendingAppointments,
            UnreadContactMessages = unreadMessages
        };
    }
}
