using System.Collections;
using System.Collections.Generic;

namespace DemoDictionary.Data.Models
{
    public class User
    {
        public User(int id, string name, string password)
        {
            this.Id = id;
            this.Name = name;
            this.Password = password;
            this.Dictionaries = new HashSet<Dictionary>();
        }

        public int Id { get; }

        public string Name { get; }

        public string Password { get; }

        public ICollection<Dictionary> Dictionaries { get; }
    }
}
