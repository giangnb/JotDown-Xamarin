using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace JotDown
{
    public interface IAuthenticate
    {
        Task<MobileServiceUser> Authenticate( MobileServiceAuthenticationProvider provider);
        void Logout();
    }
}
