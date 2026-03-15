using CraneManagementSystem.Application.DTOs.Report;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain;

namespace CraneManagementSystem.Application.Services;

public class ReportService
{
    private readonly IIncomeExpenseRepository _incomeExpenseRepository;

    public ReportService(IIncomeExpenseRepository incomeExpenseRepository)
    {
        _incomeExpenseRepository = incomeExpenseRepository;
    }

    public async Task<IncomeExpenseReportDto> GetIncomeExpenseReportAsync(ReportFilterDto filter, CancellationToken cancellationToken = default)
    {
        var all = await _incomeExpenseRepository.GetAllAsync(cancellationToken);
        var query = all.ToList().AsEnumerable();

        if (filter.StartDate.HasValue)
            query = query.Where(x => x.Date >= filter.StartDate.Value);
        if (filter.EndDate.HasValue)
            query = query.Where(x => x.Date <= filter.EndDate.Value);
        if (filter.CompanyId.HasValue)
            query = query.Where(x => x.CompanyId == filter.CompanyId);

        var list = query.ToList();
        var totalIncome = list.Where(x => x.Type == IncomeExpenseType.Gelir).Sum(x => x.Amount);
        var totalExpense = list.Where(x => x.Type == IncomeExpenseType.Gider).Sum(x => x.Amount);

        return new IncomeExpenseReportDto
        {
            TotalIncome = totalIncome,
            TotalExpense = totalExpense,
            Balance = totalIncome - totalExpense,
            Items = list.Select(x => new IncomeExpenseReportItemDto
            {
                Id = x.Id,
                Type = (int)x.Type,
                Category = x.Category,
                Amount = x.Amount,
                Date = x.Date,
                Description = x.Description
            }).ToList()
        };
    }
}
