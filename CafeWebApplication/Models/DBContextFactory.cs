using CafeWebApplication;
using CafeWebApplication.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit
{
    public class DBContextFactory : IDBContextFactory
    {
        private readonly string _connectionString;

        public DBContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DB_CafeContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DB_CafeContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new DB_CafeContext(optionsBuilder.Options);
        }
    }
}
