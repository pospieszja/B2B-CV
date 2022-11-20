using HortimexB2B.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HortimexB2B.Infrastructure.Repositories.Interfaces
{
    public interface ITermConsentRepository
    {
        Task<TermConsent> GetByUserAsync(string userName);
        Task AddConsentAsync(string userName);
    }
}
