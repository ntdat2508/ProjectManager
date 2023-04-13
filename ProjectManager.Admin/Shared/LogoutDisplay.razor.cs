using ProjectManager.Admin.Data;

namespace ProjectManager.Admin.Shared
{
    public class LogoutDisplayBase : CommonComponentBase
    {
        public void HandleClick()
        {
            token = null;
            Logout();
        }
    }
}
