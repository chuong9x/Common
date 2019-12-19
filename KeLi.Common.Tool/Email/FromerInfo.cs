namespace KeLi.Common.Tool.Email
{
    /// <summary>
    /// Former info.
    /// </summary>
    public class FromerInfo
    {
        /// <summary>
        /// News a former info instance.
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
        /// Former mail address.
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// Former display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Former password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Former host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Former port.
        /// </summary>
        public int Port { get; set; }
    }
}