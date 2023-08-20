using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace ImageCropVersion2.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult CustomCrop()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CustomCrop(string filename, IFormFile blob)
        {
            try
            {
                using (var image = Image.Load(blob.OpenReadStream()))
                {
                    string systemFileExtension = filename.Substring(filename.LastIndexOf('.'));
                    string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                    Directory.CreateDirectory(rootPath); // Ensure the directory exists or create it if not

                    // Calculate the pixel dimensions for Bangladesh passport size (300 DPI)
                    int passportWidth = 531; // Width in pixels
                    int passportHeight = 413; // Height in pixels

                    // Generate a unique file name
                    var newFileName = GenerateFileName("Passport", systemFileExtension);

                    // Construct the full file path
                    var filePath = Path.Combine(rootPath, newFileName);

                    // Resize the image to the Bangladesh passport size and save it
                    image.Mutate(x => x.Resize(passportWidth, passportHeight));
                    image.Save(filePath);
                }

                return Json(new { Message = "OK" });
            }
            catch (Exception)
            {
                return Json(new { Message = "ERROR" });
            }
        }



        public string GenerateFileName(string fileTypeName, string fileextenstion)
        {
            if (fileTypeName == null) throw new ArgumentNullException(nameof(fileTypeName));
            if (fileextenstion == null) throw new ArgumentNullException(nameof(fileextenstion));
            return $"{fileTypeName}_{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{fileextenstion}";
        }
    }
}
