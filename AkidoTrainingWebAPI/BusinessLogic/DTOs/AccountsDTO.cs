﻿using System.ComponentModel.DataAnnotations;

namespace AkidoTrainingWebAPI.BusinessLogic.DTOs
{
    public class AccountsDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
