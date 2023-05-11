using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeWebApplication.Interfaces
{
    public interface IDBContextFactory
    {
        DB_CafeContext CreateDbContext(string connectionString);
    }
}
