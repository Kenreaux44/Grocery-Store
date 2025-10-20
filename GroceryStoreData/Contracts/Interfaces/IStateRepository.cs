using GroceryStoreData.Models;

namespace GroceryStoreData.Contracts.Interfaces
{
    public interface IStateRepository
    {
        IEnumerable<State> GetAll();
        State? GetById(int id);
        State? GetByAbbreviation(string abbreviation);
        Task AddAsync(State state);
        Task UpdateAsync(State state);
        Task DeleteAsync(State state);
    }
}
