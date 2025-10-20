using MyfirstLib.Models;

namespace MyfirstLib.Contracts.Interfaces
{
    public interface IStateService
    {
        IEnumerable<StateModel> GetAll();
        StateModel? GetById(int id);
        StateModel? GetByAbbreviation(string abbreviation);
        Task AddAsync(StateModel state);
        Task UpdateAsync(StateModel state);
        Task DeleteAsync(StateModel state);
    }
}