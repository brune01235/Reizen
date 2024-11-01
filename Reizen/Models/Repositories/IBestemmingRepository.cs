﻿namespace Reizen.Models.Repositories
{
    public interface IBestemmingRepository
    {
        Task<Bestemming> GetBestemming(string Code);
        Task<List<Bestemming>> GetBestemmingen();
        Task<List<Bestemming>> GetBestemmingenByLandId(int landId);
    }
}
