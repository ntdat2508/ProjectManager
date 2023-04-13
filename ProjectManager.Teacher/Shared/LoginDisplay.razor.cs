using ProjectManager.Teacher.Data;

namespace ProjectManager.Teacher.Shared
{
    public class LoginDisplayBase : CommonComponentBase
    {
        public string username;

        protected override void OnInitialized()
        {
            username = userName;
        }
    }
}
