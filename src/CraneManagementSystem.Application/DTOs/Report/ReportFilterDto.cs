namespace CraneManagementSystem.Application.DTOs.Report;

public class ReportFilterDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? ConstructionSiteId { get; set; }
    public Guid? CraneId { get; set; }
}
