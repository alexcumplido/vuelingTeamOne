using Microsoft.EntityFrameworkCore;
using VY.Hackathon.TeamOne.WebApi.DataProvider.Entities;

namespace VY.Hackathon.TeamOne.WebApi.DataProvider.Repository
{
    public class ResultSnapshotRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ResultSnapshotRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultSnapshotEntity?> GetLatestAsync()
        {
            return await _dbContext.ResultSnapshots
                .OrderByDescending(e => e.SnapshotDateTime)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(ResultSnapshotEntity entity)
        {
            await _dbContext.AddAsync(entity);

            await _dbContext.SaveChangesAsync();
        }
    }
}
