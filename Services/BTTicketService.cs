using Microsoft.EntityFrameworkCore;
using TheBugTracker.Data;
using TheBugTracker.Models;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Services
{
    public class BTTicketService : IBTTicketService
    {
        private readonly ApplicationDbContext _context;

        public BTTicketService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task AddNewTicketAsync(Ticket ticket)
        {
            try
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            try
            {
                _context.Update(ticket);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task AddTicketAttachmentAsync(TicketAttachment ticketAttachment)
        {
            throw new NotImplementedException();
        }

        public Task AddTicketCommentAsync(TicketComment ticketComment)
        {
            throw new NotImplementedException();
        }

        public async Task ArchiveTicketAsync(Ticket ticket)
        {
            ticket.Archived = true;

            _context.Update(ticket);
            await _context.SaveChangesAsync();

        }

        public Task AssignTicketAsync(int ticketId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetAllTicketsByCompanyAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetAllTicketsByPriorityAsync(int companyId, string priorityName)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetAllTicketsByStatusAsync(int companyId, string statusName)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetAllTicketsByTypeAsync(int companyId, string typeName)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetArchivedTicketsAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetProjectTicketsByPriorityAsync(string priorityName, int companyId, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetProjectTicketsByRoleAsync(string role, string userId, int projectId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetProjectTicketsByStatusAsync(string statusName, int companyId, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetProjectTicketsByTypeAsync(string typeName, int companyId, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket> GetTicketAsNoTrackingAsync(int ticketId)
        {
            throw new NotImplementedException();
        }

        public Task<TicketAttachment> GetTicketAttachmentByIdAsync(int ticketAttchmentId)
        {
            throw new NotImplementedException();
        }

        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
        }

        public Task<BTUser> GetTicketDeveloperAsync(int ticketId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetTicketsByRoleAsync(string role, string userId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetTicketsByUserIdAsync(string userId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetUnassignedTicketsAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<int?> LookupTicketPriorityIdAsync(string priorityName)
        {
           try
            {
                TicketPriority ticketPriority = (await _context.TicketPriorities.FirstOrDefaultAsync(t => t.Name == priorityName));
                return ticketPriority?.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int?> LookupTicketStatusIdAsync(string statusName)
        {
            try
            {
                TicketStatus ticketStatus = (await _context.TicketStatuses.FirstOrDefaultAsync(t => t.Name == statusName));
                return ticketStatus?.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<int?> LookupTicketTypeIdAsync(string typeName)
        {
            throw new NotImplementedException();
        }

        public Task RestoreTicketAsync(Ticket ticket)
        {
            throw new NotImplementedException();
        }

       
    }
}
