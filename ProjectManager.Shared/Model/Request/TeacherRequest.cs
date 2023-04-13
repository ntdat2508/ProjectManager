namespace ProjectManager.Shared.Model.Request
{
    public class TeacherRequest : PagingRequest
    {
        public long? DepartmentId { get; set; }
        public long? SpecializedId { get; set; }
    }
}
