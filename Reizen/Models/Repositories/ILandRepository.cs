namespace Reizen.Models.Repositories
{
    public interface ILandRepository
    {
        Task<Land> GetLand(int id);
        Task<List<Land>> GetLanden();
    }
}
