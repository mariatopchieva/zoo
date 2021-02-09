using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zoo.Data.Models;

namespace Zoo.Data.Context
{
    public class ZooDbContext : DbContext
    {
        public ZooDbContext()
        {
        }

        public ZooDbContext(DbContextOptions<ZooDbContext> options) : base(options)
        {

        }

        public DbSet<Species> Species { get; set; }

        public DbSet<Animal> Animals { get; set; }

    }
}
