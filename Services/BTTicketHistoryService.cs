using TheBugTracker.Data;
using TheBugTracker.Models;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Services
{
    public class BTTicketHistoryService : IBTTicketHistoryService
    {

        private readonly ApplicationDbContext _context;

        public  BTTicketHistoryService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task AddHistoryAsync(Ticket oldTicket, Ticket newTicket, string userId)
        {
           // NEW TICKET HAS BEEN ADDED
           if( oldTicket == null && newTicket!= null)
            {
                TicketHistory ticketHistory = new()
                {
                    Property = "",
                    Created = DateTimeOffset.Now,
                    Description = "New Ticket Created",
                    NewValue = "",
                    OldValue = "",
                    TicketId = newTicket.Id,
                    UserId = userId,
                };

                try
                {
                    await _context.TicketHistories.AddAsync(ticketHistory);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }else
            {
                // Check Ticket Title
                
                if(oldTicket.Title != newTicket.Title)
                {
                    TicketHistory ticketHistory = new()
                    {
                        Property = "Title",
                        Created = DateTimeOffset.Now,
                        Description = $"New Ticket Title : {newTicket.Title}",
                        NewValue = newTicket.Title,
                        OldValue = oldTicket.Title,
                        TicketId = newTicket.Id,
                        UserId = userId,
                    };

                    await _context.TicketHistories.AddAsync(ticketHistory);

                }


                // Check Ticket Description

                if (oldTicket.Description != newTicket.Description)
                {
                    TicketHistory ticketHistory = new()
                    {
                        Property = "Description",
                        Created = DateTimeOffset.Now,
                        Description = $"New Ticket Description : {newTicket.Description}",
                        NewValue = newTicket.Description,
                        OldValue = oldTicket.Description,
                        TicketId = newTicket.Id,
                        UserId = userId,
                    };

                    await _context.TicketHistories.AddAsync(ticketHistory);

                }


                // Check Ticket Priority

                if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
                {
                    TicketHistory ticketHistory = new()
                    {
                        Property = "TicketPriority",
                        Created = DateTimeOffset.Now,
                        Description = $"New Ticket Priority : {newTicket.TicketPriority.Name}",
                        NewValue = newTicket.TicketPriority.Name,
                        OldValue = oldTicket.TicketPriority.Name,
                        TicketId = newTicket.Id,
                        UserId = userId,
                    };

                    await _context.TicketHistories.AddAsync(ticketHistory);

                }

                // Check Ticket Status

                if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
                {
                    TicketHistory ticketHistory = new()
                    {
                        Property = "TicketStatus",
                        Created = DateTimeOffset.Now,
                        Description = $"New Ticket Status : {newTicket.TicketStatus.Name}",
                        NewValue = newTicket.TicketStatus.Name,
                        OldValue = oldTicket.TicketStatus.Name,
                        TicketId = newTicket.Id,
                        UserId = userId,
                    };

                    await _context.TicketHistories.AddAsync(ticketHistory);

                }

                // Check Ticket Type

                if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
                {
                    TicketHistory history = new()
                    {
                        TicketId = newTicket.Id,
                        Property = "TicketType",
                        OldValue = oldTicket.TicketType.Name,
                        NewValue = newTicket.TicketType.Name,
                        Created = DateTimeOffset.Now,
                        UserId = userId,
                        Description = $"New ticket type: {newTicket.TicketType.Name}"
                    };

                    await _context.TicketHistories.AddAsync(history);
                }

                // Check Ticket Developer

                if (oldTicket.DeveloperUserId != newTicket.DeveloperUserId)
                {
                    TicketHistory history = new()
                    {
                        TicketId = newTicket.Id,
                        Property = "Developer",
                        OldValue = oldTicket.DeveloperUser?.FullName ?? "Not Assigned",
                        NewValue = newTicket.DeveloperUser?.FullName,
                        Created = DateTimeOffset.Now,
                        UserId = userId,
                        Description = $"New ticket type: {newTicket.DeveloperUser.FullName}"
                    };

                    await _context.TicketHistories.AddAsync(history);
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {

                    throw;
                }

            }

        }

        public Task<List<TicketHistory>> GetCompanyTicketsHistoriesAsync(int compnayId)
        {
            throw new NotImplementedException();
        }

        public Task<List<TicketHistory>> GetProjectTicketsHistoriesAsync(int projectId, int companyId)
        {
            throw new NotImplementedException();
        }
    }
}
