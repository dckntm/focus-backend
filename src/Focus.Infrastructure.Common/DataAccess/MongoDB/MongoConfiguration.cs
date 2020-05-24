namespace Focus.Infrastructure.Common.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class MongoConfiguration : IMongoConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Database { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Host { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int Port { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Password { get; set; }
    }
}