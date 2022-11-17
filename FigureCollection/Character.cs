using System.ComponentModel.DataAnnotations;
namespace FigureCollection
{
    public class Character
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
    }
}
