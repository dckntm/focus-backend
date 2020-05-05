using System.Collections.Generic;

namespace Focus.Application.Common.Services.Client
{
    public class ClientConfiguration
    {
        public ICollection<ServiceConfiguration> RequiredServices { get; set; }
    }

    public class ServiceConfiguration
    {
        public string Service { get; set; }
        public string Host { get; set; }
    }
}