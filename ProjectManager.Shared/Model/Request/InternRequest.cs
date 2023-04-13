namespace ProjectManager.Shared.Model.Request
{
    public class InternRequest : PagingRequest
    {

        public long? TeacherId { get; set; }
        public long? StudentId { get; set; }
        public long? SchoolYearId { get; set; }

        public long Status { get; set; }
    }
}
