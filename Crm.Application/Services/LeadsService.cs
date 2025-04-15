using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Application.DTO.Leads;
using Crm.Domain.Interfaces;
using Crm.Application.Interfaces;
using Crm.Domain.Entities;
using Crm.Application.DTO;
using System.Net.Sockets;

namespace Crm.Application.Services
{
    public class LeadsService : ILeadsService
    {
        public ILeadsRepository _leadsRepository;
        public LeadsService(ILeadsRepository leadsRepository)
        {
            _leadsRepository = leadsRepository;
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

        public async Task<string> AddLeadAsync(AddLeadsDto leads)
        {
            if (leads == null)
            {
                throw new ArgumentNullException(nameof(leads));
            }

            var leadEntity = new LeadTbl
            {
                LeadSource = leads.LeadSource,
                LeadStatus = leads.LeadStatus,
                SalesStage = leads.SalesStage,
                Product = leads.Product,
                DealName = leads.DealName,
                ExpectedCloseDate = leads.ExpectedCloseDate,
            };

            var sales = new SalesRep
            {
                AssignedSalesRep = leads.SalesRep?.AssignedSalesRep,
                FollowUpDate = leads.SalesRep?.FollowUpDate,
                NextAction = leads.SalesRep?.NextAction,
                LastContactDate = leads.SalesRep?.LastContactDate
            };

            var totalPrice = CalculateTotalPrice(leads.Payment?.Price, leads.Payment?.Discount);

            var payment = new Payment
            {
                Price = leads.Payment?.Price,
                Discount = leads.Payment?.Discount,
                TotalPrice = totalPrice,
                PaymentTerms = leads.Payment?.PaymentTerms,
                InvoiceNumber = leads.Payment?.InvoiceNumber,
                PaymentStatus = leads.Payment?.PaymentStatus
            };

            await _leadsRepository.AddLeadAsync(leadEntity, sales, payment);
            return "Lead added successfully";

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
                LeadSource = lead.LeadSource,
                LeadStatus = lead.LeadStatus,
                SalesStage = lead.SalesStage,
                Product = lead.Product,
                DealName = lead.DealName,
                ExpectedCloseDate = lead.ExpectedCloseDate,
               
                SalesRep = lead.SalesRep == null ? null : new SalesRepDto
                {
                    AssignedSalesRep = lead.SalesRep.AssignedSalesRep,
                    FollowUpDate = lead.SalesRep.FollowUpDate,
                    NextAction = lead.SalesRep.NextAction,
                    LastContactDate = lead.SalesRep.LastContactDate
                },
                Payment = lead.Payment == null ? null : new PaymentDto
                {
                    Price = lead.Payment.Price,
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
