namespace ProjectManager.Shared.Model.Request
{
    public class ClasssRequest : PagingRequest
    {
        public long? DepartmentId { get; set; }
        public long? SpecializedId { get; set; }
        public long? SchoolYearId { get; set; }
    }
}
