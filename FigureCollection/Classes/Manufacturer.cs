using System.ComponentModel.DataAnnotations;

namespace FigureCollection.Classes
{
    public class Manufacturer
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;

    }
}
