namespace Focus.Infrastructure.Common.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMongoConfiguration
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        string Database { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        string Host { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        int Port { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        string User { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        string Password { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Password))
                    return $@"mongodb://{Host}:{Port}";
                return $@"mongodb://{User}:{Password}@{Host}:{Port}";
            }
        }
    }
}