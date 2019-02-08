using BleemSync.Data;
using BleemSync.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BleemSync.Services
{
    public class GameManagerNodeService
    {
        private readonly DatabaseContext _context;

        public GameManagerNodeService(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<GameManagerNode> All()
        {
            return _context.Nodes.ToList();
        }

        public GameManagerNode Get(int id)
        {
            return _context.Nodes.Where(n => n.Id == id).FirstOrDefault();
        }

        public void Create(GameManagerNode node, bool autosave = true)
        {
            _context.Nodes.Add(node);
            
            if (autosave)
            {
                _context.SaveChanges();
            }
        }

        public void Update(GameManagerNode node, bool autosave = true)
        {
            _context.Nodes.Update(node);

            if (autosave)
            {
                _context.SaveChanges();
            }
        }

        public void Delete(int id, bool autosave = true)
        {
            Delete(Get(id), autosave);
        }

        public void Delete(GameManagerNode node, bool autosave = true)
        {
            _context.Remove(node);

            if (autosave)
            {
                _context.SaveChanges();
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
