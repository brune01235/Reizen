namespace Reizen.Models.Repositories
{
    public interface IWerelddeelRepository
    {
       Task<Werelddeel> GetWerelddeel(int id);
        Task<List<Werelddeel>> GetWerelddelen();
    }
}
