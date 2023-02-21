using VY.Hackaton.TeamOne.ProviderData.Infrastructure.Contracts.Models;

namespace VY.Hackathon.TeamOne.WebApi.Models
{
    public class ResultSnapshotModel
    {
        public int SnapshotId { get; set; }

        public DateTime SnapshotDateTime { get; set; } = DateTime.UtcNow;

        public ParameterModel? Parameters { get; set; }

        public ProviderDataResponse[]? ResultInfo { get; set; }
    }
}
