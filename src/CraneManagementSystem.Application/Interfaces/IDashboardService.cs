using CraneManagementSystem.Application.DTOs.Dashboard;

namespace CraneManagementSystem.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetSummaryAsync(Guid? companyId, CancellationToken cancellationToken = default);
}
