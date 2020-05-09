using System.Collections.Generic;

namespace Focus.Service.Identity.Core.Entities
{
    public class Organization
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsHead { get; set; }
        public IList<string> Members { get; set; }
    }
}