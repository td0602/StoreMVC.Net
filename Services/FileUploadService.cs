

namespace Store_App.Services;

public class FileUploadService
{
    private readonly IWebHostEnvironment _hostingEnv;
    public FileUploadService(IWebHostEnvironment hostingEnv) {
        _hostingEnv = hostingEnv;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string myPath)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }
        //Tạo Folder uploads
        var uploadsFolder = Path.Combine(_hostingEnv.WebRootPath, "uploads/"+myPath);
        // Generic tên file
        var newFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        // Kết hợp file name và đường dẫn tới thư mucj tạo thành path đến ảnh
        var filePath = Path.Combine(uploadsFolder, newFileName);
        Console.WriteLine(filePath.ToString());

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return newFileName;
    }
}