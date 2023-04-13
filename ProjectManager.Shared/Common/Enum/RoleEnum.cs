using System.ComponentModel;

namespace ProjectManager.Shared.Common.Enum
{
    public enum RoleEnum
    {
        [Description("Quản trị viên")]
        Admin = 0,
        [Description("Giáo viên")]
        Teacher = 1,
        [Description("Sinh viên")]
        Student = 2
    }
}
