namespace CraneManagementSystem.Application.DTOs.Dashboard;

public class DashboardSummaryDto
{
    public int TotalCranes { get; set; }
    public int ActiveCranes { get; set; }
    public int TotalCompanies { get; set; }
    public int TotalConstructionSites { get; set; }
    public int TotalOperators { get; set; }
    public decimal TotalIncomeThisMonth { get; set; }
    public decimal TotalExpenseThisMonth { get; set; }
    public int PendingAppointments { get; set; }
    public int UnreadContactMessages { get; set; }
}
