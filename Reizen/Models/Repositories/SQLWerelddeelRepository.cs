using Microsoft.EntityFrameworkCore;

namespace Reizen.Models.Repositories
{
    public class SQLWerelddeelRepository : IWerelddeelRepository
    {
        private readonly ReizenContext context;

        public SQLWerelddeelRepository(ReizenContext context)
        {
            this.context = context;
        }

        public async Task<Werelddeel> GetWerelddeel(int id)
        {
            return await context.Werelddelen.FindAsync(id);
        }

        public async Task<List<Werelddeel>> GetWerelddelen()
        {
            return await context.Werelddelen.ToListAsync();
        }
    }
}
