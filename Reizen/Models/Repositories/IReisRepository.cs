namespace Reizen.Models.Repositories
{
    public interface IReisRepository
    {
        Task<Reis> GetReis(int id);
        Task<List<Reis>> GetReizen();
        Task<List<Reis>> GetReizenByBestemmingCode(string bestemmingCode);

        Task UpdateReis(Reis reis);

    }
}
