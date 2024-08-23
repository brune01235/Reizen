namespace Reizen.Models.Repositories
{
    public interface IKlantRepository
    {
        Task<Klant> GetKlant(int id);
        Task<List<Klant>> GetKLanten();
    }
}
