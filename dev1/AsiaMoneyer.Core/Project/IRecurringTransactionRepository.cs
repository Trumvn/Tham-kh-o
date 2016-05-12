using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Entities;

namespace AsiaMoneyer.Project
{
    public interface IRecurringTransactionRepository : IRepository<RecurringTransaction, string>
    {
    }
}
