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

        // GET: api/Accounts/ViewAccount/5
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

        // PUT: api/Accounts/Update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAccount(int id, AccountsDTOPut accounts)
        {
            var accountToUpdate = await _repository.GetAccountsByIdAsync(id);
            if (accountToUpdate == null)
            {
                return NotFound();
            }

            if (await _repository.IsEmailExistsAsync(accounts.Email))
            {
                return Conflict("This email is already used for other accounts");
            }

            if (accounts.Role.ToString() == "Admin" || accounts.Role.ToString() == "User" || accounts.Role.ToString() == "Head Admin")
            {
                accountToUpdate.Email = accounts.Email;
                accountToUpdate.Role = accounts.Role;
                accountToUpdate.Name = accounts.Name;
                accountToUpdate.Password = accounts.Password;
                accountToUpdate.Level = accounts.Level;
                accountToUpdate.Belt = accounts.Belt;

                await _repository.UpdateUserAsync(accountToUpdate);
                return NoContent();
            }
            else
            {
                return BadRequest("Must be Admin or User");
            }
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
        [HttpPost("Login")]
        public async Task<ActionResult> Login(AccountsDTOLogin login)
        {
            var existingEmail = await _repository.GetAccountsByEmailAsync(login.Email);

            if (existingEmail == null || existingEmail.Password != login.Password)
            {
                return NotFound("Invalid email or password.");
            }

            return Ok(existingEmail.Role);
        }

        [HttpPost("Register")]
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
                Role = "User",
                Level = 5,
                Belt = "Black",
                ImagePath = "Default.jpg"
            };
            await _repository.AddAccountsAsync(newAccount);
            return CreatedAtAction(nameof(GetAccounts), new { id = newAccount.Id}, newAccount);
        }

        [HttpGet("Image/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var account = await _repository.GetAccountsByIdAsync(id);

            // Directory where uploaded images are stored
            var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "API", "Avatar");

            // Full path to the requested image
            var imagePath = Path.Combine(uploadsDirectory, account.ImagePath);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound("Image not found.");
            }

            // Return the image file
            var imageFileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            return File(imageFileStream, "image/jpeg"); // Adjust the content type as needed
        }

        [HttpPut("UploadImages/{id}")]
        public async Task<IActionResult> EditProfilePicture(int id, IFormFile avatar)
        {
            var accountToUpdate = await _repository.GetAccountsByIdAsync(id);

            if (accountToUpdate == null)
            {
                return NotFound("User not found");
            }

            if (accountToUpdate.ImagePath != "Default.jpg")
            {
                DeleteAvatar(accountToUpdate.ImagePath);
            }
            accountToUpdate.ImagePath = await WriteFile(avatar, accountToUpdate.Name);
            await _repository.UpdateUserAsync(accountToUpdate);

            return Ok(accountToUpdate.ImagePath);
        }

        private async Task<string> WriteFile(IFormFile image, string userName)
        {
            string filename = "";
            try
            {
                var extension = "." + image.FileName.Split('.')[image.FileName.Split('.').Length - 1];
                filename = userName + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), $"API\\Avatar");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), $"API\\Avatar", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
            }
            return filename;
        }

        private void DeleteAvatar(string avatarPath)
        {
            var deleteFile = Path.Combine(Directory.GetCurrentDirectory(), $"API\\Avatar\\", avatarPath);
            if (System.IO.File.Exists(deleteFile))
            {
                System.IO.File.Delete(deleteFile);
            }
            else
            {
                throw new Exception("File not found");
            }
        }
    }
}
