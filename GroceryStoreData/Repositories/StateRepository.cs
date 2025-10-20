using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Data;
using GroceryStoreData.Models;

namespace GroceryStoreData.Repositories
{
    public class StateRepository : IStateRepository
    {
        private readonly GroceryStore_DataContext _context;

        public StateRepository(
            GroceryStore_DataContext context
        )
        {
            _context = context;
        }

        public async Task AddAsync(State state)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            _context.States.Add(state);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(State state)
        {
            if (state is null)
            {
                return;
            }

            _context.States.Remove(state);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<State> GetAll()
        {
            return _context.States
                .ToList();
        }

        public State? GetByAbbreviation(string abbreviation)
        {
            return _context.States
                .FirstOrDefault(x => x.Abbreviation == abbreviation);
        }

        public State? GetById(int id)
        {
            return _context.States
                .FirstOrDefault(x => x.StateId == id);
        }

        public async Task UpdateAsync(State state)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            _context.States.Update(state);
            await _context.SaveChangesAsync();
       }
    }
}
