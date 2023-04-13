using ProjectManager.Student.Data;

namespace ProjectManager.Student.Shared
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
