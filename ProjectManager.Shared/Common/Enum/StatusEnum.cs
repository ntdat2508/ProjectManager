using System.ComponentModel;

namespace ProjectManager.Shared.Common.Enum
{
    public enum StatusEnum
    {
        [Description("Tất cả")]
        All = 0,
        [Description("Không đạt")]
        Fail = 1,
        [Description("Đạt")]
        Pass = 2
    }
}
