using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;

namespace AsiaMoneyer
{
    public abstract class AppServiceBase
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public ILogger Logger { get; set; }

        public UnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        public String UserId { get; set; }
    }
}
