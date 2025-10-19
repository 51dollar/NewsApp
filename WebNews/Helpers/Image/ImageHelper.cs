namespace WebNews.Helpers.Image;

public class ImageHelper
{
    private readonly IWebHostEnvironment _webHost;
    private readonly string[] _allowedExtension = { ".jpg", ".jpeg", ".png" };

    public ImageHelper(IWebHostEnvironment webHost)
    {
        _webHost = webHost;
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var inputFileExtension = Path.GetExtension(file.FileName);
        var fileName = Guid.NewGuid() + inputFileExtension;
        var filePath = Path.Combine(_webHost.WebRootPath, "Image", fileName);

        try
        {
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }
        catch (Exception e)
        {
            return "Error uploading file: " + e.Message;
        }

        return $"/Image/{fileName}";
    }

    public void DeleteFile(string fileName)
    {
        if (string.IsNullOrEmpty(fileName) || fileName == "/Image/default.png")
        {
            return;
        }
        
        string cleanPath = fileName.TrimStart('/','\\');
        
        string filePath = Path.Combine(_webHost.WebRootPath, cleanPath);
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public bool ValidFileExtension(IFormFile file)
    {
        var inputFile = Path.GetExtension(file.FileName).ToLower();
        bool isAllowed = _allowedExtension.Contains(inputFile);

        return isAllowed;
    }
}