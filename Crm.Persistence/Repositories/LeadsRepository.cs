using Crm.Domain.Entities;
using Crm.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Crm.Persistence.Repositories
{
    public class LeadsRepository : ILeadsRepository
    {
        CrmDbContext _context;

        public LeadsRepository(CrmDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<LeadTbl>, int)> GetAllLeadsAsync(int pageNumber, int pageSize)
        {
            var leadsQuery = _context.LeadTbl
                .Include(lead => lead.DealTbl)
                .Include(lead => lead.Payment)
                .AsNoTracking();

            int totalRecords = await leadsQuery.CountAsync();  // Get total count before pagination

            // Apply pagination
            var paginatedLeads = await leadsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (paginatedLeads, totalRecords);
        }

        public async Task<LeadTbl> GetLeadByIdAsync(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.LeadTbl
                .Include(lead => lead.DealTbl)
                .Include(lead => lead.Payment)
                .AsNoTracking()
                .FirstOrDefaultAsync(lead => lead.LeadId == id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<IEnumerable<LeadTbl>> SearchLeadAsync(string search)
        {
            if (string.IsNullOrEmpty(search))
                return new List<LeadTbl>();

            // Searching by names and company names
            return await _context.LeadTbl
                .Where(leads =>
                    EF.Functions.Like(leads.FullName, $"%{search}%") ||
                    (leads.CompanyName != null && EF.Functions.Like(leads.CompanyName, $"%{search}%")) ||
                    (leads.Email != null && EF.Functions.Like(leads.Email, $"%{search}%"))
                )
                .ToListAsync();
        }
        public async Task AddLeadAsync(LeadTbl lead, DealTbl deals, Payment payment)
        {
            if (lead == null)
            {
                throw new ArgumentNullException(nameof(lead));
            }
            await _context.LeadTbl.AddAsync(lead);
            await _context.SaveChangesAsync();

            if (deals != null)
            {
                deals.LeadId = lead.LeadId; // Set the foreign key relationship
                await _context.DealTbl.AddAsync(deals);
            }
            if (payment != null)
            {
                payment.LeadId = lead.LeadId; // Set the foreign key relationship
                await _context.Payment.AddAsync(payment);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateLeadAsync(LeadTbl lead)
        {
            _context.LeadTbl.Update(lead);
            await _context.SaveChangesAsync();
        }
    }
}
