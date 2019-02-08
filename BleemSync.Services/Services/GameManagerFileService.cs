using BleemSync.Data;
using BleemSync.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BleemSync.Services
{
    public class GameManagerFileService
    {
        private readonly DatabaseContext _context;

        public GameManagerFileService(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<GameManagerFile> All()
        {
            return _context.Files.ToList();
        }

        public GameManagerFile Get(int id)
        {
            return _context.Files.Where(n => n.Id == id).FirstOrDefault();
        }

        public void Create(GameManagerFile file, bool autosave = true)
        {
            _context.Files.Add(file);

            if (autosave)
            {
                _context.SaveChanges();
            }
        }

        public void Update(GameManagerFile file, bool autosave = true)
        {
            _context.Files.Update(file);

            if (autosave)
            {
                _context.SaveChanges();
            }
        }

        public void Delete(int id, bool autosave = true)
        {
            Delete(Get(id), autosave);
        }

        public void Delete(GameManagerFile file, bool autosave = true)
        {
            _context.Remove(file);

            if (autosave)
            {
                _context.SaveChanges();
            }
        }
    }
}
