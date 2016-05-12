using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer
{
    public interface IDbContextProvider<out TDbContext> //where TDbContext : System.Data.Entity.DbContext
    {
        TDbContext DbContext { get; }
    }
}
