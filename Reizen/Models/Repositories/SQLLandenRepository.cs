﻿using Microsoft.EntityFrameworkCore;

namespace Reizen.Models.Repositories
{
    public class SQLLandenRepository : ILandRepository
    {
        private readonly ReizenContext context;

        public SQLLandenRepository(ReizenContext context)
        {
            this.context = context;
        }

        public async Task<Land> GetLand(int id)
        {
            return await context.Landen.FindAsync(id);
        }

        public async Task<List<Land>> GetLanden()
        {
            return await context.Landen.ToListAsync();
        }
    }
}