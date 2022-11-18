using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FigureCollection.Classes
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string googleId { get; set; }
    }
}
