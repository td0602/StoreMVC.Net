

namespace Store_App.Services;

public class FileUploadService
{
    private readonly IWebHostEnvironment _hostingEnv;
    public FileUploadService(IWebHostEnvironment hostingEnv) {
        _hostingEnv = hostingEnv;
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }

        var uploadsFolder = Path.Combine(_hostingEnv.WebRootPath, "uploads/BookImages");
        var newFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        var filePath = Path.Combine(uploadsFolder, newFileName);
        Console.WriteLine(filePath.ToString());

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return newFileName;
    }
}