using VY.Hackaton.TeamOne.ProviderData.Infrastructure.Contracts.Models;

namespace VY.Hackathon.TeamOne.WebApi.Models
{
    public class ResultSnapshotInfo
    {
        public ProviderDataRequest Parameters { get; set; }

        public ProviderDataResponse[] DataResponse { get; set; }
    }
}
