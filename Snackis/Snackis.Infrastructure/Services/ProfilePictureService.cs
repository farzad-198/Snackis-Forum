using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
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

        public async Task<string> SaveAsync(
            Stream imageStream,
            CancellationToken cancellationToken = default)
        {
            using MemoryStream uploadedImage = new();

            byte[] buffer = new byte[8192];

            while (true)
            {
                int bytesRead = await imageStream.ReadAsync(
                    buffer,
                    0,
                    buffer.Length,
                    cancellationToken);

                if (bytesRead == 0)
                {
                    break;
                }

                if (uploadedImage.Length + bytesRead > MaxFileSize)
                {
                    throw new ArgumentException(
                        "The image cannot exceed 2 MB.");
                }

                uploadedImage.Write(buffer, 0, bytesRead);
            }

            if (uploadedImage.Length == 0)
            {
                throw new ArgumentException(
                    "Please select an image.");
            }

            uploadedImage.Position = 0;

            IImageFormat format = await Image.DetectFormatAsync(
                uploadedImage,
                cancellationToken);

            if (format.Name != JpegFormat.Instance.Name
                && format.Name != PngFormat.Instance.Name
                && format.Name != WebpFormat.Instance.Name)
            {
                throw new ArgumentException(
                    "Only JPEG, PNG and WebP images are allowed.");
            }

            uploadedImage.Position = 0;

            ImageInfo info = await Image.IdentifyAsync(
                new DecoderOptions
                {
                    SkipMetadata = true,
                    MaxFrames = 1
                },
                uploadedImage,
                cancellationToken);

            if (info.Width <= 0 || info.Height <= 0
                || info.Width > 4096 || info.Height > 4096
                || (long)info.Width * info.Height > 12_000_000)
            {
                throw new ArgumentException(
                    "The image dimensions are too large or invalid.");
            }

            uploadedImage.Position = 0;

            using Image image = await Image.LoadAsync(
                new DecoderOptions { MaxFrames = 1 },
                uploadedImage,
                cancellationToken);

            image.Mutate(context => context.AutoOrient().Resize(
                new ResizeOptions
                {
                    Size = new Size(256, 256),
                    Mode = ResizeMode.Crop
                }));

            Directory.CreateDirectory(_storageDirectory);

            string fileName =
                Guid.NewGuid().ToString("N") + ".png";

            string filePath =
                Path.Combine(_storageDirectory, fileName);

            bool fileCreated = false;

            try
            {
                await using (FileStream output = new(
                    filePath,
                    FileMode.CreateNew,
                    FileAccess.Write,
                    FileShare.None))
                {
                    fileCreated = true;

                    await image.SaveAsync(
                        output,
                        new PngEncoder { SkipMetadata = true },
                        cancellationToken);
                }
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