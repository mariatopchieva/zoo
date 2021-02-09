using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zoo.Data.Context;

namespace Zoo.Tests
{
    public class Utils
    {
        public static DbContextOptions<ZooDbContext> GetOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<ZooDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }
    }
}
