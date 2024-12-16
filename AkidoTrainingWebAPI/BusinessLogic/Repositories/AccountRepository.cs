using AkidoTrainingWebAPI.BusinessLogic.DTOs;
using AkidoTrainingWebAPI.DataAccess.Data;
using AkidoTrainingWebAPI.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AkidoTrainingWebAPI.BusinessLogic.Repositories
{
    public class AccountRepository
    {
        private readonly AkidoTrainingWebAPIContext _context;
        public AccountRepository(AkidoTrainingWebAPIContext context)
        {
            _context = context;
        }
        
        public async Task<ActionResult<IEnumerable<Accounts>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }
        
        public async Task<Accounts> GetAccountsByIdAsync(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<Accounts> GetAccountsByEmailAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Accounts.AnyAsync(a => a.Email == email);
        }

        public async Task AddAccountsAsync(AccountsDTO account)
        {
            var newAccount = new Accounts
            {
                Id = account.Id,
                Name = account.Name,
                Password = account.Password,
                Email = account.Email,
                Role = account.Role
            };
            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();
        }
        
        public async Task UpdateUserAsync(Accounts account)
        {
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public bool IsAccountExisting(int id)
        {
            return _context.Accounts.Any(a => a.Id == id);
        }
    }
}
