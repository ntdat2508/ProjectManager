using ProjectManager.Admin.Data;

namespace ProjectManager.Admin.Shared
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
