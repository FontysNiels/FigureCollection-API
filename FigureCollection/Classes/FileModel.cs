using Microsoft.AspNetCore.Mvc;

namespace FigureCollection.Classes
{
    public class FileModel
    {
        public string FileName { get; set; }
        public IFormFile file { get; set; }

    }
}
