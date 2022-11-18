using System.ComponentModel.DataAnnotations;

namespace FigureCollection.Classes
{
    public class Figure
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public Brand Brand { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public Character Character { get; set; }
        public Line Line { get; set; }
        public Edition Edition { get; set; }
        public int size { get; set; }
        public int scale { get; set; }

        //WillCascadeOnDelete()

    }
}
