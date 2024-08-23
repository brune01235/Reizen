using Microsoft.EntityFrameworkCore;

namespace Reizen.Models.Repositories
{
    public class SQLBoekingRepository : IBoekingRepository
    {
        private readonly ReizenContext context;

        public SQLBoekingRepository(ReizenContext context)
        {
            this.context = context;
        }

        public async Task<Boeking> GetBoeking(int id)
        {
            return await context.Boekingen.FindAsync(id);
        }

        public async Task<List<Boeking>> GetBoekingen()
        {
            return await context.Boekingen.ToListAsync();
        }

        public async Task AddBoeking(Boeking boeking)
        {
            context.Boekingen.Add(boeking);
            await context.SaveChangesAsync();
        }
    }
}
