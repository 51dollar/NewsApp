namespace WebNews.Helpers;

public class UploadFileToFolder
{
    private readonly IWebHostEnvironment _webHost;

    public UploadFileToFolder(IWebHostEnvironment webHost)
    {
        _webHost = webHost;
    }

    public async Task<string> uploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return "";

        var inputFileExtension = Path.GetExtension(file.FileName);
        var fileName = Guid.NewGuid().ToString() + inputFileExtension;
        var filePath = Path.Combine(_webHost.WebRootPath, "Image", fileName);

        try
        {
            await using (var fileStram = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStram);
            }
        }
        catch (Exception e)
        {
            return "Error uploading file: " + e.Message;
        }

        return $"/Image/{fileName}";
    }
}