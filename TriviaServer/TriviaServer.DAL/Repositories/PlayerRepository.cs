using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaServer.DAL.Repositories
{
    public class PlayerRepository
    {
        private readonly TriviaRankContext _context;
        public PlayerRepository(TriviaRankContext context)
        {
            _context = context;
        }
    }
}
