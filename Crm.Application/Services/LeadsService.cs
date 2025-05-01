using Crm.Application.DTO;
using Crm.Application.DTO.Leads;
using Crm.Application.Interfaces;
using Crm.Domain.Entities;
using Crm.Domain.Interfaces;

namespace Crm.Application.Services
{
    public class LeadsService : ILeadsService
    {
        public readonly ILeadsRepository _leadsRepository;
        public readonly IClientRepository _clientRepository;

        public LeadsService(ILeadsRepository leadsRepository, IClientRepository clientRepository)
        {
            _leadsRepository = leadsRepository;
            _clientRepository = clientRepository;
        }

        public async Task<PaginatedResponse<LeadsDto>> GetAllLeadsAsync(int pageNumber, int pageSize)
        {
            var (leads, totalCount) = await _leadsRepository.GetAllLeadsAsync(pageNumber, pageSize);
            // var leads = await _leadsRepository.GetAllLeadsAsync(pageNumber, pageSize);

            if (leads == null || !leads.Any())
                return new PaginatedResponse<LeadsDto>(new List<LeadsDto>(), totalCount, pageNumber, pageSize);
            var leadsDto = leads.Select(MapToDto);

            return new PaginatedResponse<LeadsDto>(leadsDto, totalCount, pageNumber, pageSize);
        }

        public async Task<IEnumerable<LeadsDto>> GetLeadByIdAsync(int id)
        {
            var lead = await _leadsRepository.GetLeadByIdAsync(id);
            if (lead == null)
            {
                throw new KeyNotFoundException($"Lead with ID {id} not found.");
            }
            return new List<LeadsDto> { MapToDto(lead) };
        }

        public async Task<IEnumerable<LeadsDto>> SearchLeadsAsync(string search)
        {
            var leads = await _leadsRepository.SearchLeadAsync(search);

            var leadsResult = leads.Select(MapToDto).ToList();
            return leadsResult;
        }

        public async Task<string> AddLeadAsync(AddLeadsDto leads)
        {
            if (leads == null)
            {
                throw new ArgumentNullException(nameof(leads));
            }

            //get clients data
            Clients? client = null;

            if (leads.ClientID != null)
            {
                // Fetch client data using ClientID
                client = await _clientRepository.GetClientsByIdAsync(leads.ClientID.Value);

                //validation for archived clients
                if (client == null || client.IsArchived)
                {
                    return "Cannot add lead";
                }
            }

            var leadEntity = new LeadTbl
            {
                ClientID = leads.ClientID,
                FullName = client != null ? $"{client.FirstName} {client.MiddleName ?? ""} {client.LastName}".Trim() : null,
                Email = client?.Email,
                PhoneNumber = client?.PhoneNumber,
                CompanyName = client?.CompanyDetails?.CompanyName,
                Industry = client?.CompanyDetails?.IndustryType,
                LeadSource = leads.LeadSource,
                Status = leads.Status
            };

            var deals = new DealTbl
            {
                DealName = leads.Deals?.DealName,
                DealValue = leads.Deals?.DealValue,
                Currency = leads.Deals?.Currency,
                Stage = leads.Deals?.Stage,
                AssignedSalesRep = leads.Deals?.AssignedSalesRep,
                Status = leads.Deals?.Status,
                Notes = leads.Deals?.Notes,
            };

            var totalPrice = CalculateTotalPrice(leads.Payment?.EstimatedValue, leads.Payment?.Discount);

            var payment = new Payment
            {
                EstimatedValue = leads.Payment?.EstimatedValue,
                Discount = leads.Payment?.Discount,
                TotalPrice = totalPrice,
                PaymentTerms = leads.Payment?.PaymentTerms,
                InvoiceNumber = leads.Payment?.InvoiceNumber,
                PaymentStatus = leads.Payment?.PaymentStatus
            };

            await _leadsRepository.AddLeadAsync(leadEntity, deals, payment);
            return "Lead added successfully";
        }

        public async Task<string> UpdateLeadAsync(int leadId, UpdateLeadsDto update)
        {
            var lead = await _leadsRepository.GetLeadByIdAsync(leadId);
            if (lead == null) { return "Lead not found"; }

            UpdateLead(lead, update);
            UpdateDeal(lead.DealTbl ??= new DealTbl(), update);
            UpdatePayment(lead.Payment ??= new Payment(), update);

            await _leadsRepository.UpdateLeadAsync(lead);
            return "Lead updated successfully";
        }

        private void UpdateLead(LeadTbl lead, UpdateLeadsDto update)
        {
            if (!string.IsNullOrWhiteSpace(update.LeadSource))
                lead.LeadSource = update.LeadSource;

            if (!string.IsNullOrWhiteSpace(update.Status))
                lead.Status = update.Status;
        }

        private void UpdateDeal(DealTbl deal, UpdateLeadsDto update)
        {
            if (!string.IsNullOrEmpty(update.Currency))
                deal.Currency = update.Currency;

            if (!string.IsNullOrEmpty(update.DealName))
                deal.DealName = update.DealName;

            if (update.DealValue.HasValue && update.DealValue > 0)
                deal.DealValue = update.DealValue;

            if (!string.IsNullOrEmpty(update.Stage))
                deal.Stage = update.Stage;

            if (!string.IsNullOrEmpty(update.AssignedSalesRep))
                deal.AssignedSalesRep = update.AssignedSalesRep;

            if (!string.IsNullOrEmpty(update.DealStatus))
                deal.Status = update.DealStatus;
        }

        private void UpdatePayment(Payment payment, UpdateLeadsDto update)
        {
            if (update.EstimatedValue.HasValue && update.EstimatedValue > 0)
                payment.EstimatedValue = update.EstimatedValue;

            if (update.Discount.HasValue && update.Discount > 0)
                payment.Discount = update.Discount;

            // Calculate total price
            payment.TotalPrice = CalculateTotalPrice(payment.EstimatedValue, payment.Discount);

            if (!string.IsNullOrEmpty(update.PaymentTerms))
                payment.PaymentTerms = update.PaymentTerms;

            if (!string.IsNullOrEmpty(update.InvoiceNumber))
                payment.InvoiceNumber = update.InvoiceNumber;

            if (!string.IsNullOrEmpty(update.PaymentStatus))
                payment.PaymentStatus = update.PaymentStatus;
        }

        private decimal CalculateTotalPrice(decimal? price, decimal? discount)
        {
            var actualPrice = price ?? 0;
            var discountPercentage = discount ?? 0;
            return actualPrice - (actualPrice * (discountPercentage / 100));
        }

        private LeadsDto MapToDto(LeadTbl lead)
        {
            return new LeadsDto
            {
                LeadId = lead.LeadId,
                FullName = lead.FullName,
                Email = lead.Email,
                PhoneNumber = lead.PhoneNumber,
                CompanyName = lead.CompanyName,
                Industry = lead.Industry,
                LeadSource = lead.LeadSource,
                Status = lead.Status,
                DateCreated = lead.DateCreated,

                Deals = lead.DealTbl == null ? null : new DealsDto
                {
                    DealName = lead.DealTbl.AssignedSalesRep,
                    DealValue = lead.DealTbl.DealValue,
                    Currency = lead.DealTbl.Currency,
                    Stage = lead.DealTbl.Stage,
                    AssignedSalesRep = lead.DealTbl.AssignedSalesRep,
                    Status = lead.DealTbl.Status,
                    Notes = lead.DealTbl.Notes
                },

                Payment = lead.Payment == null ? null : new PaymentDto
                {
                    EstimatedValue = lead.Payment.EstimatedValue,
                    Discount = lead.Payment.Discount,
                    TotalPrice = lead.Payment.TotalPrice,
                    PaymentTerms = lead.Payment.PaymentTerms,
                    InvoiceNumber = lead.Payment.InvoiceNumber,
                    PaymentStatus = lead.Payment.PaymentStatus
                }
            };
        }

    }
}
