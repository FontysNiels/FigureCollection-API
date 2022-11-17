using System.ComponentModel.DataAnnotations;
namespace FigureCollection
{
    public class Edition
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        
    }
}
