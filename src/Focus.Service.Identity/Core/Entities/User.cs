using System.Collections.Generic;
using Focus.Core.Common.Abstract;
using Focus.Service.Identity.Enums;

namespace Focus.Service.Identity.Entities
{
    public class User : ValueObject
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Username;
            yield return Password;
            yield return Role;
        }
    }
}