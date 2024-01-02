namespace TradingPOC.LoginAPI.Models
{
    public class UserInfo
    {
        public string Type { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string lastName { get; set; }
        public string Email { get; set; }
    }
    public class UserLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
