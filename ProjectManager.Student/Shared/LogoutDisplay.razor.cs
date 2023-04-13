using ProjectManager.Student.Data;

namespace ProjectManager.Student.Shared
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
