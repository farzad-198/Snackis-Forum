using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Snackis.Core.Interfaces
{
    public interface IProfilePictureService
    {
        Task<string> SaveAsync(
            Stream imageStream,
            CancellationToken cancellationToken = default);
    }
}