namespace CMS.Models
{
    public class SessionVariable
    {
        public const string SessionKeyUserId = "SessionKeyUserId";
        public const string SessionKeyUserEmail = "SessionKeyUserEmail";
        public const string SessionKeyUserRoleId = "SessionKeyUserRoleId";
        public const string SessionKeySessionId = "SessionKeySessionId";
        public const string SessionKeyMessageType = "SessionKeyMessageType";
        public const string SessionKeyMessage = "SessionKeyMessage";
        public const string SessionKeyIsPost = "SessionKeyIsPost";

        public enum SessionKeyName
        {
            SessionKeyUserId = 0,
            SessionKeyUserEmail = 1,
            SessionKeyUserRoleId = 2,
            SessionKeySessionId = 3,
            SessionKeyMessageType = 4,
            SessionKeyMessage = 5,
            SessionKeyIsPost = 6,
        }
    }
}
