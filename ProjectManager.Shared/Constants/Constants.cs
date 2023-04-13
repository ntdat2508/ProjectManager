namespace ProjectManager.Shared.Constants
{
    public class Constants
    {
        public static readonly string ConnectionStrings = "ConnectionStrings";
        public static readonly string MainConnectionString = "MainConnectionString";
        public static string SortDesc = "desc";
        public static string SortId = "Id";
        public static string Key = "@Key";
        public static string Page = "@Page";
        public static string PageSize = "@PageSize";
        public static string DepartmentId = "@DepartmentId";
        public static string SpecializedId = "@SpecializedId";
        public static string SchoolYearId = "@SchoolYearId";
        public static string StudentId = "@StudentId";

        public static string Status = "@Status";
        public static string ClasssId = "@ClasssId";
        public static string TeacherId = "@TeacherId";
        public const string ApplicationJson = "application/json";
        public const string Bearer = "Bearer";
        public const string Yes = "Có";
        public const string No = "Không";
        public const string NumberOne = "1";
        public const string Source = "/";
        public const string Zero = "0";
        public const string Notify = "Thông báo";
        public const string Warning = "Cảnh báo";
        public const string DateFormat = "dd/MM/yyyy hh:mm:ss";
        public const string DateFormat2 = "dd/MM/yyyy";

        public static class Message
        {
            public static readonly string Successfully = "Thành công";
            public static readonly string InternalServer = "Internal Server Error";
            public static readonly string ModelStateMessage = "Model State Invalid";
            public static readonly string IdNotFound = "Không tìm thấy Id";
            public static readonly string Validation = "Dữ liệu có chứa kí tự không hợp lệ";
            public static readonly string Idexist = "Mã số đã tồn tại";

            public static readonly string RecordNotFoundMessage = "Không tìm thấy bản ghi";
            public static readonly string DeleteSuccess = "Xóa thông tin thành công";
            public static readonly string DeleteFail = "Đã xảy ra lỗi trong quá trình xóa dữ liệu. Vui lòng kiểm tra lại";
            public static readonly string SaveSuccess = "Lưu thông tin thành công";
            public static readonly string SaveFail = "Đã xảy ra lỗi trong quá trình lưu dữ liệu. Vui lòng kiểm tra lại";
            public static readonly string UserInActiveMessage = "Tài khoản hoặc mật khẩu không đúng. Vui lòng thử lại";
            public static readonly string ImportSuccess = "Import dữ liệu thành công";
            public static readonly string FileCorrectFormat = "File tải lên chưa đúng định dạng. Hệ thống chỉ tiếp nhận file có đuôi là csv";
            public static readonly string FileNotFound = "Không tìm thấy File";
            public static readonly string FileNull = "Dữ liệu trong File trống. Vui lòng tạo dữ liệu trong File";
            public static readonly string Duplicate = "Dữ liệu này đã tồn tại. Vui lòng nhập dữ liệu khác";
            public static readonly string Fail = "Lỗi";
            public const string ConfirmDelete = "Bạn có chắc chắn muốn xóa bản ghi này không?";
        }

        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }
        }

        public static class FromDelete
        {
            public const string Classs = "Classs";
            public const string Department = "Department";
            public const string ProjectList = "ProjectList";
            public const string SchoolYear = "SchoolYear";
            public const string Specialized = "Specialized";
            public const string Student = "Student";
            public const string Intern = "Intern";

            public const string Teacher = "Teacher";
            public const string TrainingSystem = "TrainingSystem";
        }
    }
}
