using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Application.DTO.Clients;
using Crm.Domain.Entities;
using Crm.Domain.Interfaces;
using Crm.Application.Interfaces;
using Crm.Domain;
using Crm.Application.DTO;

namespace Crm.Application.Services
{
    public class ClientsService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientsService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<PaginatedResponse<ClientsDto>> GetAllClientAsync(bool ascending, bool sortByRecentlyAdded, ClientFilters filters, int pageNumber, int pageSize)
        {
            var (clients, totalRecords) = await _clientRepository.GetAllClientsAsync(ascending, sortByRecentlyAdded, filters, pageNumber, pageSize);

            if (clients == null || !clients.Any())
                return new PaginatedResponse<ClientsDto>(new List<ClientsDto>(), totalRecords, pageNumber, pageSize);

            var clientsDto = clients.Select(client => MapToClientsDto(client)).ToList();
            return new PaginatedResponse<ClientsDto>(clientsDto, totalRecords, pageNumber, pageSize);
        }

        public async Task<IEnumerable<ClientsDto>> GetClientInfoById(int clientId)
        {
            var client = await _clientRepository.GetClientsByIdAsync(clientId);
            if (client == null)
                return new List<ClientsDto>();

            return new List<ClientsDto> { MapToClientsDto(client, true) };
        }

        public async Task<IEnumerable<ClientsDto>> GetAllArchieveClientAsync()
        {
            var clients = await _clientRepository.GetAllArchieveAsync();
            if (clients == null || !clients.Any())
                return new List<ClientsDto>();

            //calling the MapToClientsDto method to map the clients to ClientsDto
            var clientsDto = clients.Select(client => MapToClientsDto(client)).ToList();
            return clientsDto;

        }

        public async Task<IEnumerable<ClientsDto>> SearchClientAsync(string? name)
        {
            var clients = await _clientRepository.SearchClientsAsync(name);

            var clientsDto = clients.Select(client => MapToClientsDto(client, true)).ToList();
            return clientsDto;

        }

        public async Task<string> AddClientAsync(AddClientsDto addClient)
        {
            var clients = new Clients
            {
                PhotoLink = addClient.PhotoLink,
                FirstName = addClient.FirstName,
                MiddleName = addClient.MiddleName,
                LastName = addClient.LastName,
                PhoneNumber = addClient.PhoneNumber,
                Email = addClient.Email,
                WebsiteURL = addClient.WebsiteUrl,
                IsArchived = false,
            };

            var company = addClient.Company != null ? new CompanyDetails
            {
                CompanyName = addClient.Company.CompanyName,
                IndustryType = addClient.Company.IndustryType,
                BusinessRegNumber = addClient.Company.BusinessRegNumber,
                CompanySize = addClient.Company.CompanySize,
                City = addClient.Company.City,
                StateProvince = addClient.Company.StateProvince,
                ZipCode = addClient.Company.ZipCode,
                Country = addClient.Company.Country,
            } : null;

            var contact = addClient.Contact?.Select(c => new ContactPerson
            {
                ContactName = c.ContactName,
                JobTitle = c.JobTitle,
                Department = c.Department,
                DirectPhone = c.DirectPhoneNumber,
                DirectEmail = c.DirectEmail,
            }).ToList() ?? new List<ContactPerson>();

            var commentEntities = new List<Comments>();//list of comment
            if (addClient.Details?.Notes != null && addClient.Details.Notes.Any())
            {
                commentEntities = addClient.Details.Notes
                    .Select(note => new Comments
                    {
                        Content = note,
                        CreatedAt = DateTimeOffset.UtcNow,
                    })
                    .ToList();
            }

            var details = addClient.Details != null ? new ClientDetails
            {
                LeadSources = addClient.Details.LeadSources,
                ClientType = addClient.Details.ClientType,
                Notes = commentEntities,//the notes are added and saved on Comments table
            } : null;

            await _clientRepository.AddClientsAsync(clients, company, contact, details);
            return "Client added successfully";
        }

        public async Task<string> AddCommentsToClientAsync(int clientId, string content)
        {
            var client = await _clientRepository.GetClientsByIdAsync(clientId);
            if (client == null) { return "Client not found"; }

            /*var comment = new Comments
            {
                Content = content,
                //CreatedAt = DateTime.UtcNow,
                //ClientDetailsId = clientDetails.ClientDetailsId
            };*/

            await _clientRepository.AddCommentToClientAsync(clientId, content);

            return "Comment added successfully";
        }

        public async Task<string> UpdateClientAsync(int clientId, UpdateClientsDto request)
        {
            var client = await _clientRepository.GetClientsByIdAsync(clientId);
            if (client == null)
                return "Client not found";

            // Update photo if provided
            if (!string.IsNullOrWhiteSpace(request.PhotoLink))
                client.PhotoLink = request.PhotoLink;

            if (!string.IsNullOrWhiteSpace(request.FullName))
            {
                var nameParts = request.FullName
                    .Trim()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                client.FirstName = nameParts.ElementAtOrDefault(0);
                client.MiddleName = nameParts.Length == 3 ? nameParts[1] : null;
                client.LastName = nameParts.Length >= 2 ? nameParts.Last() : null;
            }


            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                client.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(request.Email))
                client.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.WebsiteURL))
                client.WebsiteURL = request.WebsiteURL;

            // Update or add ContactPerson
            if (!string.IsNullOrWhiteSpace(request.ContactName) ||
                !string.IsNullOrWhiteSpace(request.JobTitle) ||
                !string.IsNullOrWhiteSpace(request.Department) ||
                !string.IsNullOrWhiteSpace(request.DirectEmail) ||
                !string.IsNullOrWhiteSpace(request.DirectPhoneNumber))
            {
                if (client.ContactPerson != null && client.ContactPerson.Any())
                {
                    var contact = client.ContactPerson.First();
                    if (!string.IsNullOrWhiteSpace(request.ContactName)) contact.ContactName = request.ContactName;
                    if (!string.IsNullOrWhiteSpace(request.JobTitle)) contact.JobTitle = request.JobTitle;
                    if (!string.IsNullOrWhiteSpace(request.Department)) contact.Department = request.Department;
                    if (!string.IsNullOrWhiteSpace(request.DirectEmail)) contact.DirectEmail = request.DirectEmail;
                    if (!string.IsNullOrWhiteSpace(request.DirectPhoneNumber)) contact.DirectPhone = request.DirectPhoneNumber;
                }
                else
                {
                    client.ContactPerson = new List<ContactPerson>
                    {
                        new ContactPerson
                        {
                            ContactName = request.ContactName,
                            JobTitle = request.JobTitle,
                            Department = request.Department,
                            DirectEmail = request.DirectEmail,
                            DirectPhone = request.DirectPhoneNumber
                        }
                    };
                }
            }

            // Ensure company details exist
            if (client.CompanyDetails == null)
                client.CompanyDetails = new CompanyDetails();

            if (!string.IsNullOrWhiteSpace(request.CompanyName))
                client.CompanyDetails.CompanyName = request.CompanyName;

            if (!string.IsNullOrWhiteSpace(request.IndustryType))
                client.CompanyDetails.IndustryType = request.IndustryType;

            if (!string.IsNullOrWhiteSpace(request.BusinessRegNumber))
                client.CompanyDetails.BusinessRegNumber = request.BusinessRegNumber;

            if (!string.IsNullOrWhiteSpace(request.CompanySize))
                client.CompanyDetails.CompanySize = request.CompanySize;

            //update address
            if (!string.IsNullOrWhiteSpace(request.CompanyAddress))
            {
                var addressParts = request.CompanyAddress
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(part => part.Trim())
                    .ToArray();

                if (addressParts.Length < 4)
                {
                    return "Invalid company address format. Expected format: 'ZipCode, City, StateProvince, Country'";
                }

                client.CompanyDetails.ZipCode = addressParts.ElementAtOrDefault(0);
                client.CompanyDetails.City = addressParts.ElementAtOrDefault(1);
                client.CompanyDetails.StateProvince = addressParts.ElementAtOrDefault(2);
                client.CompanyDetails.Country = addressParts.ElementAtOrDefault(3);
            }

            await _clientRepository.UpdateClientsAsync(client);
            return "Client updated successfully";
        }



        public async Task<string> IsArchivedClientAsync(bool isArchived, int clientId)
        {
            var client = await _clientRepository.GetClientsByIdAsync(clientId);
            if (client == null) { return "Client not found"; }
            await _clientRepository.IsArchivedClientsAsync(isArchived, clientId);

            if (isArchived == true)
            {
                return "Client archived successfully";
            }
            else
            {
                return "Client unarchived successfully";

            }
        }

        private ClientsDto MapToClientsDto(Clients client, bool includeDetails = false)
        {
            var dto = new ClientsDto
            {
                ClientId = client.ClientID,
                PhotoLink = client.PhotoLink,
                FullName = $"{client.FirstName} {client.MiddleName} {client.LastName}",
                PhoneNumber = client.PhoneNumber,
                Email = client.Email,
                CompanyName = client.CompanyDetails?.CompanyName
            };

            if (includeDetails)
            {
                dto.WebsiteURL = client.WebsiteURL;
                dto.CompanyDetails = new CompanyDetailsDto
                {
                    CompanyName = client.CompanyDetails?.CompanyName,
                    IndustryType = client.CompanyDetails?.IndustryType,
                    BusinessRegNumber = client.CompanyDetails?.BusinessRegNumber,
                    CompanySize = client.CompanyDetails?.CompanySize,
                    CompanyAddress = $"{client.CompanyDetails?.ZipCode}, {client.CompanyDetails?.City}, {client.CompanyDetails?.StateProvince}, {client.CompanyDetails?.Country}",
                };
                dto.ContactPerson = client.ContactPerson?.Select(cp => new ContactPersonDto
                {
                    ContactName = cp.ContactName,
                    JobTitle = cp.JobTitle,
                    Department = cp.Department,
                    DirectPhone = cp.DirectPhone,
                    DirectEmail = cp.DirectEmail,
                }).ToList();
                dto.ClientDetails = new ClientDetailsDto
                {
                    LeadSources = client.ClientDetails?.LeadSources,
                    ClientType = client.ClientDetails?.ClientType,
                    Notes = client.ClientDetails?.Notes?
                        .Select(n => new NoteDto
                        {
                            Content = n.Content,
                            CreatedAt = n.CreatedAt
                        }).ToList(),
                }; 
            }

            return dto;
        }


        /*public async Task<string> UnarchivedClientAsync(int clientId)
        {
            var client = await _clientRepository.GetClientsByIdAsync(clientId);
            if (client == null) { return "Client not found"; }

            client.IsArchived = false;
            await _clientRepository.UnarchivedClientsAsync(client);
            return "Client unarchived successfully";
        }*/

        /*public async Task<IEnumerable<ClientsDto>> GetRecentlyAddedClientAsync()
        {
            var clients = await _clientRepository.GetRecentlyAddedClientsAsync();
            return clients.Select(clients => new ClientsDto
            {
                ClientId = clients.ClientID,
                FullName = $"{clients.FirstName} {clients.MiddleName} {clients.LastName}",
                PhoneNumber = clients.PhoneNumber,
                Email = clients.Email,
                CompanyName = clients.CompanyDetails?.CompanyName,
            }).ToList();
        }
        public async Task<IEnumerable<ClientsDto>> GetClientSortedByNameAsync(bool ascending)
        {
            var clients = await _clientRepository.GetClientsSortedByNameAsync(ascending);
            return clients.Select(clients => new ClientsDto
            {
                ClientId = clients.ClientID,
                FullName = $"{clients.FirstName} {clients.MiddleName} {clients.LastName}",
                PhoneNumber = clients.PhoneNumber,
                Email = clients.Email,
                CompanyName = clients.CompanyDetails?.CompanyName
            }).ToList();
        }

        public async Task<IEnumerable<ClientsDto>> GetClientByIndustryAsync(string industryType)
        {
            var clients = await _clientRepository.GetClientsByIndustryAsync(industryType);
            return clients.Select(clients => new ClientsDto
            {
                ClientId = clients.ClientID,
                FullName = $"{clients.FirstName} {clients.MiddleName} {clients.LastName}",
                PhoneNumber = clients.PhoneNumber,
                Email = clients.Email,
                CompanyName = clients.CompanyDetails?.CompanyName,

            }).ToList();
        }*/

    }
}
