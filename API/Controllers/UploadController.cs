using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
   // [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        private readonly string _invoiceFileStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "invoice-uploads", "invoice-attachments");
        public UploadController()
        {
            try
            {
                if (!Directory.Exists(_storagePath))
                {
                    Directory.CreateDirectory(_storagePath);
                }
            }
            catch (Exception ex)
            {
                // Logger.LogError(ex, "Error creating storage directory.");
                throw new ApplicationException("An error occurred while setting up the storage directory.", ex);
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_storagePath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
                return Ok(new { Url = fileUrl });
            }
            catch (Exception ex)
            {
                // Log exception (optional)
                // Logger.LogError(ex, "Error uploading file.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while uploading the file.");
            }
        }

        [HttpPost("api/uploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (!Directory.Exists(_invoiceFileStoragePath))
            {
                Directory.CreateDirectory(_invoiceFileStoragePath);
            }
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var fileExtension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{fileExtension}";

            var filePath = Path.Combine(_invoiceFileStoragePath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{uniqueFileName}";
            var fileUrl = $"{Request.Scheme}://{Request.Host}/invoice-uploads/invoice-attachments/{uniqueFileName}";

            return Ok(new { Url = fileUrl });
        }
    }
}
