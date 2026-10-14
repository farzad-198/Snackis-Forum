using Snackis.Core.Interfaces;

namespace Snackis.Infrastructure.Services
{
    public class ProfilePictureService : IProfilePictureService
    {
        private const int MaxFileSize = 2 * 1024 * 1024;

        private readonly string _storageDirectory;

        public ProfilePictureService(string storageDirectory)
        {
            _storageDirectory = Path.GetFullPath(storageDirectory);
        }

        public async Task<string> SaveAsync(Stream imageStream,CancellationToken cancellationToken = default)
        {
            using MemoryStream uploadedImage = new();
            byte[] buffer = new byte[8192];

            while (true)
            {
                int bytesRead = await imageStream.ReadAsync(
                    buffer, 0, buffer.Length, cancellationToken);

                if (bytesRead == 0)
                {
                    break;
                }

                if (uploadedImage.Length + bytesRead > MaxFileSize)
                {
                    throw new ArgumentException("The image cannot exceed 2 MB.");
                }

                uploadedImage.Write(buffer, 0, bytesRead);
            }

            if (uploadedImage.Length == 0)
            {
                throw new ArgumentException("Please select an image.");
            }

            // Check the file signature, not the uploaded file name.
            byte[] data = uploadedImage.GetBuffer();
            string extension;

            if (uploadedImage.Length >= 8
                && data[0] == 0x89 && data[1] == 0x50
                && data[2] == 0x4E && data[3] == 0x47
                && data[4] == 0x0D && data[5] == 0x0A
                && data[6] == 0x1A && data[7] == 0x0A)
            {
                extension = ".png";
            }
            else if (uploadedImage.Length >= 3
                && data[0] == 0xFF
                && data[1] == 0xD8
                && data[2] == 0xFF)
            {
                extension = ".jpg";
            }
            else if (uploadedImage.Length >= 12
                && data[0] == 0x52 && data[1] == 0x49
                && data[2] == 0x46 && data[3] == 0x46
                && data[8] == 0x57 && data[9] == 0x45
                && data[10] == 0x42 && data[11] == 0x50)
            {
                extension = ".webp";
            }
            else
            {
                throw new ArgumentException("Only JPEG, PNG and WebP images are allowed.");
            }

            Directory.CreateDirectory(_storageDirectory);

            string fileName = Guid.NewGuid().ToString("N") + extension;
            string filePath = Path.Combine(_storageDirectory, fileName);
            bool fileCreated = false;

            try
            {
                await using FileStream output = new(
                    filePath,
                    FileMode.CreateNew,
                    FileAccess.Write,
                    FileShare.None);

                fileCreated = true;
                uploadedImage.Position = 0;

                await uploadedImage.CopyToAsync(
                    output, cancellationToken);
            }
            catch
            {
                if (fileCreated)
                {
                    File.Delete(filePath);
                }

                throw;
            }

            return "/uploads/profile-pictures/" + fileName;
        }
    }
}