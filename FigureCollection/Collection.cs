using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FigureCollection
{
    public class Collection
    {
        [Key]
        public int id { get; set; }
        
        [Key, ForeignKey("User")]
        public User user { get; set; }
        
        [Key, ForeignKey("Figure")]
        public Figure figure { get; set; }


    }
}
