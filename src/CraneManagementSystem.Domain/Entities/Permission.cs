namespace CraneManagementSystem.Domain.Entities;

public class Permission : BaseEntity
{
    public string ModuleName { get; set; } = string.Empty;
    public string ActionName { get; set; } = string.Empty; // View, Create, Update, Delete, Export
}
