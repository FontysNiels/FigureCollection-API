using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FigureCollection
{
    public class FigureImage
    {
        [Key]
        public int id { get; set; }

        [Key, ForeignKey("FigureId")]
        public Figure figure { get; set; }
        public string ImgData { get; set; }

    }
}
