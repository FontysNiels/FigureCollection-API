using FigureCollection.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FigureCollection.Controllers
{
    [Route("api/FigureImages")]
    [ApiController]
    public class FigureImageController : ControllerBase
    {
        private readonly DataContext _context;
        public FigureImageController(DataContext context)
        {
            _context = context;
        }

        // GET: api/FigureImages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FigureImage>>> GetFigureImages()
        {
            return Ok(await _context.FigureImages
                .Include(x => x.figure)
                    .Include(x => x.figure.Brand)
                    .Include(x => x.figure.Manufacturer)
                    .Include(x => x.figure.Character)
                    .Include(x => x.figure.Line)
                    .Include(x => x.figure.Edition).ToListAsync());
        }

        //Get FigureImage from figureid
        [HttpGet("{figureId}")]
        public async Task<ActionResult<IEnumerable<FigureImage>>> GetFigureImageByFigure(int figureId)
        {

            var selectedFigureImage = await _context.FigureImages
                .Include(x => x.figure)
                    .Include(x => x.figure.Brand)
                    .Include(x => x.figure.Manufacturer)
                    .Include(x => x.figure.Character)
                    .Include(x => x.figure.Line)
                    .Include(x => x.figure.Edition)
                .Where(us => us.figure.id == figureId).ToListAsync();

            if (selectedFigureImage.Count == 0)
            {
                return NotFound("L");
            }

            return selectedFigureImage;
        }

        [HttpGet("PreviewImages/")]
        public async Task<ActionResult<IEnumerable<FigureImage>>> getAllFistImages()
        {
            //var s = await _context.FigureImages.Include(x => x.figure).GroupBy(o => o.figure.id).Select(g => g.OrderByDescending(o => o.ImgData).FirstOrDefault()).ToListAsync();
            var s = await _context.FigureImages.Include(x => x.figure).GroupBy(o => o.figure.id).Select(g => g.FirstOrDefault()).ToListAsync();
            return Ok(s);
        }

        [HttpGet("imagename/{imagename}")]
        public IActionResult GetImageByFileName(string imagename)
        {
            return PhysicalFile(@"D:\Fontys\s3\Projecten\IP\Applicaties\Backend\FigureCollection\Images\"+ imagename, "image/jpg");
        }

        [HttpPost("{figureId}")]
        public async Task<ActionResult<FigureImage>> UploadFile(int figureId, [FromForm] FileModel fileModel)
        {

            var checkFigure = await _context.Figures.FirstOrDefaultAsync(us => us.id == figureId);
            var checkFigureImage = _context.FigureImages.Any(m => m.figure.id == figureId && m.ImgData == fileModel.FileName);

            if (checkFigure == null)
            {
                return BadRequest("Figure does not Excists.");
            }
            if (checkFigureImage == true)
            {
                return BadRequest("This Image Is Already Used For This Figure.");
            }

            try
            {
                string path = Path.Combine(@"D:\Fontys\s3\Projecten\IP\Applicaties\Backend\FigureCollection\Images", fileModel.FileName);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    fileModel.file.CopyTo(stream);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Sumtin Feked up" + ex.Message);
            }

            FigureImage newItem = new FigureImage()
            {
                figure = checkFigure,
                ImgData = fileModel.FileName
            };

            _context.FigureImages.Add(newItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFigureImages", new { id = newItem.id }, newItem);

        }

        // DELETE: api/FigureImage/5
        [HttpDelete]
        public async Task<IActionResult> DeleteFigureImage(int figure, string image)
        {
            var checkFigureImage = _context.FigureImages.Any(m => m.figure.id == figure && m.ImgData == image);

            if (checkFigureImage == false)
            {
                return NotFound("Notfound");
            }

            FigureImage removedItem = _context.FigureImages.FirstOrDefault(m => m.figure.id == figure && m.ImgData == image);
            _context.FigureImages.Remove(removedItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }


  

    }
}
