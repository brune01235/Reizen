﻿using Microsoft.EntityFrameworkCore;

namespace Reizen.Models.Repositories
{
    public class SQLKlantRepository : IKlantRepository
    {
        private readonly ReizenContext context;

        public SQLKlantRepository(ReizenContext context)
        {
            this.context = context;
        }

        public async Task<Klant> GetKlant(int id)
        {
            return await context.Klanten.Include(klant => klant.Woonplaats)
                                        .FirstOrDefaultAsync(klant => klant.Id == id);
        }

        public async Task<List<Klant>> GetKLanten()
        {
            return await context.Klanten.ToListAsync();
        }
    }
}