namespace Focus.Service.Gateway
{
    public class ServiceConfiguration
    {
        // by convention its service host at the same time
        public string Service { get; set; }
        // by convention we have default port 
        public int Port { get; set; } = 5000;
        // list of available routes for service
        public Request[] Routes { get; set; }
        public string Base => $"http://{Service}:{Port}";
    }

    public class Request
    {
        public string Method { get; set; }
        public string Route { get; set; }
    }
}