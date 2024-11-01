﻿using Microsoft.EntityFrameworkCore;

namespace Reizen.Models.Repositories
{
    public class SQLBestemmingRepository : IBestemmingRepository
    {
        private readonly ReizenContext context;

        public SQLBestemmingRepository(ReizenContext context)
        {
            this.context = context;
        }

        public async Task<Bestemming> GetBestemming(string Code)
        {
            return await context.Bestemmingen.FindAsync(Code);
        }

        public async Task<List<Bestemming>> GetBestemmingen()
        {
            return await context.Bestemmingen.ToListAsync();
        }

        public async Task<List<Bestemming>> GetBestemmingenByLandId(int landId)
        {
            return await context.Bestemmingen.Where(bestemming => bestemming.Landid == landId).OrderBy(bestemming => bestemming.Plaats).ToListAsync();
        }
    }
}

