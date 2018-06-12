using microCommerce.Common;
using microCommerce.Mvc.Controllers;
using microCommerce.Services.Media;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace microCommerce.MediaApi.Controllers
{
    public class PictureController : ServiceBaseController
    {
        #region Fields
        private readonly IPictureProvider _pictureProvider;
        #endregion

        #region Ctor
        public PictureController(IPictureProvider pictureProvider)
        {
            _pictureProvider = pictureProvider;
        }
        #endregion

        #region Methods
        [HttpGet("/pictures/{Id:int}")]
        public virtual IActionResult GetPictureById(int Id)
        {
            return Json(_pictureProvider.GetPictureById(Id));
        }
        
        [HttpGet("/pictures")]
        public virtual IActionResult GetPictures()
        {
            var pictures = _pictureProvider.GetPictures();

            return Json(pictures.Select(x => new
            {
                name = x.TitleAttribute,
                mimeType = x.MimeType,
                pictureUrl = _pictureProvider.GetPictureUrl(x)
            }));
        }
        
        [HttpPost("/pictures/upload")]
        public virtual async Task<IActionResult> Upload()
        {
            if (Request.Form == null || !Request.Form.Files.Any())
                return Content("File not selected");

            var file = Request.Form.Files[0];
            var fileBinary = new byte[file.Length];
            using (MemoryStream stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                fileBinary = stream.ToArray();
            }

            string fileName = Path.GetFileName(file.FileName);
            string fileExtension = Path.GetExtension(file.FileName);
            string contentType = MimeTypeMap.GetMimeType(fileExtension);
            if (!string.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            //insert picture
            var picture = _pictureProvider.InsertPicture(fileBinary, contentType, null);

            return Json(new
            {
                success = true,
                pictureId = picture.Id,
                pictureUrl = _pictureProvider.GetPictureUrl(picture)
            });
        }
        #endregion
    }
}