using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Group
    {
        [Key]
        public string Name { get; set; }

        public ICollection<Connection> Connections { get; set; } = new List<Connection>();

        public Group(string name)
        {
            Name = name;
        }

        // to satisfy EF when we create the DB, we need to give this an empty constructor, since we can't expect the connection infos at start
        public Group()
        {
            
        }
    }
}
