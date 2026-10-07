using Snackis.Core.Entities;
using Snackis.Core.Interfaces;

namespace Snackis.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly IRepository<Report> _reportRepository;
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<AppUser> _userRepository;

        public ReportService(
            IRepository<Report> reportRepository,
            IRepository<Post> postRepository,
            IRepository<AppUser> userRepository)
        {
            _reportRepository = reportRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public async Task<List<Report>> GetReportsAsync()
        {
            List<Report> reports =
                await _reportRepository.FindAsync(
                    report => true,
                    report => report.Post,
                    report => report.Reporter);

            return reports
                .OrderBy(report => report.IsReviewed)
                .ThenByDescending(report => report.CreatedAt)
                .ThenByDescending(report => report.Id)
                .ToList();
        }

        public async Task AddReportAsync(
            int postId,
            string reporterId,
            string reason)
        {
            if (postId <= 0)
            {
                throw new ArgumentException(
                    "The post ID is invalid.");
            }

            if (string.IsNullOrWhiteSpace(reporterId))
            {
                throw new ArgumentException(
                    "The reporter account is required.");
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException(
                    "Please enter a reason for the report.");
            }

            reason = reason.Trim();

            if (reason.Length > 1000)
            {
                throw new ArgumentException(
                    "The reason cannot exceed 1000 characters.");
            }

            Post? post =
                await _postRepository.GetByIdAsync(postId);

            if (post == null)
            {
                throw new ArgumentException(
                    "The post no longer exists.");
            }

            List<AppUser> users =
                await _userRepository.FindAsync(
                    user => user.Id == reporterId);

            if (users.Count == 0)
            {
                throw new ArgumentException(
                    "The reporter account was not found.");
            }

            Report report = new()
            {
                PostId = post.Id,
                ReporterId = reporterId,
                Reason = reason,
                CreatedAt = DateTime.UtcNow,
                IsReviewed = false,
                ReviewedAt = null
            };

            await _reportRepository.AddAsync(report);

            await _reportRepository.SaveChangesAsync();
        }

        public async Task MarkAsReviewedAsync(int reportId)
        {
            Report? report =
                await _reportRepository.GetByIdAsync(reportId);

            if (report == null)
            {
                throw new ArgumentException(
                    "The report was not found.");
            }

            if (report.IsReviewed)
            {
                return;
            }

            DateTime? previousReviewedAt = report.ReviewedAt;

            report.IsReviewed = true;
            report.ReviewedAt = DateTime.UtcNow;

            try
            {
                _reportRepository.Update(report);

                await _reportRepository.SaveChangesAsync();
            }
            catch
            {
                report.IsReviewed = false;
                report.ReviewedAt = previousReviewedAt;

                throw;
            }
        }
    }
}