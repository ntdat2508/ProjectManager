﻿namespace ProjectManager.Authentication.Model
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
