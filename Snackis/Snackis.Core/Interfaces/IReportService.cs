using Snackis.Core.Entities;

namespace Snackis.Core.Interfaces
{
    public interface IReportService
    {
        Task<List<Report>> GetReportsAsync();

        Task AddReportAsync(
            int postId,
            string reporterId,
            string reason);

        Task MarkAsReviewedAsync(
            int reportId);
    }
}