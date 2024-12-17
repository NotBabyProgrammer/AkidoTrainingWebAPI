using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using AkidoTrainingWebAPI.BusinessLogic.Repositories;
using AkidoTrainingWebAPI.BusinessLogic.DTOs;

namespace AkidoTrainingWebAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountRepository _repository;
        public AccountsController(AccountRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult> GetAccounts()
        {
            var accounts = await _repository.GetAccounts();
            return Ok(accounts);
        }

        // GET: api/Accounts/ViewAccounts/5
        [HttpGet("ViewAccounts/{id}")]
        public async Task<ActionResult> GetAccounts(int id)
        {
            var accounts = await _repository.GetAccountsByIdAsync(id);

            if (accounts == null)
            {
                return NotFound();
            }

            return Ok(accounts);
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccounts(int id, AccountsDTOPut accounts)
        {
            var accountToUpdate = await _repository.GetAccountsByIdAsync(id);
            if (accountToUpdate == null)
            {
                return NotFound();
            }
            
            accountToUpdate.Email = accounts.Email;
            accountToUpdate.Role = accounts.Role;
            accountToUpdate.Name = accounts.Name;
            accountToUpdate.Password = accounts.Password;

            await _repository.UpdateUserAsync(accountToUpdate);
            return NoContent();
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccounts(int id)
        {
            if (!_repository.IsAccountExisting(id))
            {
                return NotFound();
            }
            await _repository.DeleteAccountAsync(id);
            return NoContent();
        }

        // POST: api/Accounts/login
        [HttpPost("login")]
        public async Task<ActionResult> Login(AccountsDTOLogin login)
        {
            var existingEmail = await _repository.GetAccountsByEmailAsync(login.Email);

            if (existingEmail == null || existingEmail.Password != login.Password)
            {
                return NotFound("Invalid email or password.");
            }

            return Ok();
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(AccountsDTORegister account)
        {
            if (await _repository.IsEmailExistsAsync(account.Email))
            {
                return Conflict("This email is already used for other accounts");
            }
            var newAccount = new AccountsDTO
            {
                Name = account.Name,
                Password = account.Password,
                Email = account.Email,
                Role = "User"
            };
            await _repository.AddAccountsAsync(newAccount);
            return CreatedAtAction(nameof(GetAccounts), new { id = newAccount.Id}, newAccount);
        }
    }
}
