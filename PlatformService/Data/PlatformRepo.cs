using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;

        public PlatformRepo(AppDbContext conntext)
        {
            _context = conntext;    
        }
        public void CreatePlatform(Platform platForm)
        {
            if(platForm is null)
            {
                throw new ArgumentNullException(nameof(platForm));
            }
            _context.Platforms.Add(platForm);
        }

        public IEnumerable<Platform> GetAllPlatForms()
        {
            return _context.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}