namespace Reizen.Models.Repositories
{
    public interface IBoekingRepository
    {
        Task<Boeking> GetBoeking(int id);
        Task<List<Boeking>> GetBoekingen();

        Task AddBoeking(Boeking boeking, Reis reis);
    }
}
