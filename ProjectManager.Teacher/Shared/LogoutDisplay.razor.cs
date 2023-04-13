using ProjectManager.Teacher.Data;

namespace ProjectManager.Teacher.Shared
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
