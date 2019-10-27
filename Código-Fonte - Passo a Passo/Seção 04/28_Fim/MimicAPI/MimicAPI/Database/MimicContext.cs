using Microsoft.EntityFrameworkCore;
using MimicAPI.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Database
{
    public class MimicContext : DbContext
    {
        public MimicContext(DbContextOptions<MimicContext> options) : base(options)
        {

        }

        public DbSet<Palavra> Palavras { get; set; }
    }
}
