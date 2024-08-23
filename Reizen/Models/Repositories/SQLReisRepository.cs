using Microsoft.EntityFrameworkCore;

namespace Reizen.Models.Repositories
{
    public class SQLReisRepository : IReisRepository
    {
        private readonly ReizenContext context;

        public SQLReisRepository(ReizenContext context)
        {
            this.context = context;
        }

        public async Task<Reis> GetReis(int id)
        {
            return await context.Reizen.FindAsync(id);
        }

        public async Task<List<Reis>> GetReizen()
        {
            return await context.Reizen.ToListAsync();
        }

        public async Task<List<Reis>> GetReizenByBestemmingCode(string bestemmingCode)
        {
            return await context.Reizen.Where(reis => reis.Bestemmingscode == bestemmingCode).OrderBy(reis => reis.Vertrek).ToListAsync();
        }

        public async Task UpdateReis(Reis reis)
        {
            context.Reizen.Update(reis);
            await context.SaveChangesAsync();
        }
    }
}
