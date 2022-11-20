using HortimexB2B.Core.Domain;
using HortimexB2B.Infrastructure.EF;
using HortimexB2B.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HortimexB2B.Infrastructure.Repositories
{
    public class TermConsentRepository : ITermConsentRepository
    {
        private readonly B2BContext _context;

        public TermConsentRepository(B2BContext context)
        {
            _context = context;
        }

        public async Task AddConsentAsync(string userName)
        {
            var consent = new TermConsent();
            consent.UserName = userName;
            consent.ConsentedAt = DateTime.UtcNow;

            _context.TermConsents.Add(consent);
            await _context.SaveChangesAsync();
        }

        public async Task<TermConsent> GetByUserAsync(string userName)
        {
            return await _context.TermConsents.SingleOrDefaultAsync(x => x.UserName == userName);
        }
    }
}
