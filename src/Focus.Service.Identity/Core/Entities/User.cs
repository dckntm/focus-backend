using System.Collections.Generic;
using Focus.Core.Common.Abstract;
using Focus.Service.Identity.Core.Enums;

namespace Focus.Service.Identity.Core.Entities
{
    public class User : ValueObject
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Organization { get; set; }
        public UserRole Role { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Surname;
            yield return Patronymic;
            yield return Organization;
            yield return Username;
            yield return Password;
            yield return Role;
        }
    }
}