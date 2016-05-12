using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Dtos;

namespace AsiaMoneyer.Application
{
    public interface IApplicationAppService : IAppService
    {
        NavigatorDto GetNavigator(string userId);
    }
}
