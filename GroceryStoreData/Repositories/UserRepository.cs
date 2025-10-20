using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Data;
using GroceryStoreData.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreData.Repositories;

public class UserRepository : IUserRepository
{
    private readonly GroceryStore_DataContext _context;

    public UserRepository(GroceryStore_DataContext context)
    {
        _context = context;
    }

    public User? GetById(int id)
    {
        return _context.Users
            .Include(u => u.ShoppingLists)
            .FirstOrDefault(u => u.UserId == id);
    }

    public List<User> GetAll()
    {
        return _context.Users
            .Include(u => u.ShoppingLists)
            .ToList();
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        if (user is null)
        {
            return;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}