namespace ProjectManager.Shared.Model.Request
{
    public class ProjectListRequest : PagingRequest
    {
        public long? TeacherId { get; set; }
        public long? SchoolYearId { get; set; }
        public long Status { get; set; }
    }
}
