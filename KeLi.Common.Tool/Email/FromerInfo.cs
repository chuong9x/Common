namespace KeLi.Common.Tool.Email
{
    /// <summary>
    ///     Fromer info.
    /// </summary>
    public class FromerInfo
    {
        /// <summary>
        ///     News a Fromer info instance.
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="displayName"></param>
        /// <param name="password"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public FromerInfo(string fromAddress, string displayName, string password, string host, int port)
        {
            FromAddress = fromAddress;
            DisplayName = displayName;
            Password = password;
            Host = host;
            Port = port;
        }

        /// <summary>
        ///     Fromer mail address.
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        ///     Fromer display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     Fromer password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Fromer host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        ///     Fromer port.
        /// </summary>
        public int Port { get; set; }
    }
}