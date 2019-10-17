using System.Collections;
using System.Runtime.InteropServices.ComTypes;

namespace DemoDictionary.Data.Models
{
    public class Dictionary
    {
        public Dictionary(int id, string name, int userId)
        {
            this.Id = id;
            this.Name = name;
            this.UserId = userId;
        }

        public int Id { get; }

        public string Name { get; }

        public int UserId { get; }
    }
}
