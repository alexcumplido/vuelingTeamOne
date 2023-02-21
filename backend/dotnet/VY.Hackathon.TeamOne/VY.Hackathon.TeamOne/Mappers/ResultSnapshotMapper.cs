using Newtonsoft.Json;
using VY.Hackathon.TeamOne.WebApi.DataProvider.Entities;
using VY.Hackathon.TeamOne.WebApi.Models;
using VY.Hackaton.TeamOne.ProviderData.Infrastructure.Contracts.Models;

namespace VY.Hackathon.TeamOne.WebApi.Mappers
{
    public static class ResultSnapshotMapper
    {
        public static ResultSnapshotEntity ToEntities(this ProviderDataResponse[] model)
        {
            return new ResultSnapshotEntity
            {
                SnapshotDateTime = DateTime.UtcNow,
                SnapshotDataJson = JsonConvert.SerializeObject(model)
            };
        }

        public static ResultSnapshotModel ToModel(this ResultSnapshotEntity entity)
        {
            var snapshotInfo = JsonConvert.DeserializeObject<ResultSnapshotInfo>(entity.SnapshotDataJson);

            return new ResultSnapshotModel
            {
                SnapshotDateTime = entity.SnapshotDateTime,
                Parameters = snapshotInfo?.Parameters,
                ResultInfo = snapshotInfo?.DataResponse
            };
        }
    }
}
