namespace ProjectManager.Authentication.Model
{
    public class AuthenticateResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public string Token { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public int ExpireTime { get; set; }
    }
}
