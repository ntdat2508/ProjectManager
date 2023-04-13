namespace ProjectManager.Shared.Model.Request
{
    public class StudentRequest : PagingRequest
    {
        public long? DepartmentId { get; set; }
        public long? SpecializedId { get; set; }
        public long? ClasssId { get; set; }
    }
}
