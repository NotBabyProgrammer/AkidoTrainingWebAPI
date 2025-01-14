using AkidoTrainingWebAPI.DataAccess.Data;
using AkidoTrainingWebAPI.DataAccess.Models;
using AkidoTrainingWebAPI.BusinessLogic.DTOs.AreasDTO;
using Microsoft.EntityFrameworkCore;

namespace AkidoTrainingWebAPI.BusinessLogic.Repositories
{
    public class AreasRepository
    {
        private readonly AkidoTrainingWebAPIContext _context;
        public AreasRepository(AkidoTrainingWebAPIContext context)
        {
            _context = context;
        }
        public async Task AddAreasAsync(AreasDTO area)
        {
            var newArea = new Areas
            {
                Id = area.Id,
                Name = area.Name,
                District = area.District,
                Schedule = area.Schedule,
                Address = area.Address,
                ImagePath = area.ImagePath
            };
            _context.Areas.Add(newArea);
            await _context.SaveChangesAsync();
        }
    }
}
