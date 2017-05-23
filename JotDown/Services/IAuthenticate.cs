using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace JotDown.Services
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate( MobileServiceAuthenticationProvider provider);
    }
}
