using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TheBugTracker.Data;
using TheBugTracker.Models;
using TheBugTracker.Models.Enums;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Services
{
    public class BTTicketService : IBTTicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTRolesService _rolesService;
        private readonly IBTProjectService _projectService;

        public BTTicketService(ApplicationDbContext context, IBTRolesService rolesService)
        {
            _context = context;
            _rolesService = rolesService;
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

        public async Task AssignTicketAsync(int ticketId, string userId)
        {
            Ticket ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);

            try
            {
                if (ticket != null)
                {
                    ticket.DeveloperUserId = userId;
                    ticket.TicketStatusId = (await LookupTicketStatusIdAsync("Development")).Value;
                    await _context.SaveChangesAsync();

                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<Ticket>> GetAllTicketsByCompanyAsync(int companyId)
        {
          
            try
            {

                List<Ticket> tickets = await _context.Projects
                                       .Where(p => p.Id == companyId)
                                       .SelectMany(p => p.Tickets)
                                                          .Include(t => t.Attachments)
                                                          .Include(t => t.Comments)
                                                          .Include(t => t.DeveloperUser)
                                                          .Include(t => t.History)
                                                          .Include(t => t.OwnerUser)
                                                          .Include(t => t.TicketPriority)
                                                          .Include(t => t.TicketStatus)
                                                          .Include(t => t.TicketType)
                                                          .Include(t => t.Project)
                                        .ToListAsync();

                return tickets;

            }catch(Exception)
            {
                throw;
            }


        }

        public async Task<List<Ticket>> GetAllTicketsByPriorityAsync(int companyId, string priorityName)
        {

            int TicketPriorityId = (await LookupTicketPriorityIdAsync(priorityName)).Value;

            try
            {
                List<Ticket> tickets = await _context.Projects
                                                      .Where(p => p.CompanyId == companyId)
                                                      .SelectMany(p => p.Tickets)
                                                            .Include(t => t.Attachments)
                                                            .Include(t => t.Comments)
                                                            .Include(t => t.DeveloperUser)
                                                            .Include(t => t.History)
                                                            .Include(t => t.OwnerUser)
                                                            .Include(t => t.TicketPriority)
                                                            .Include(t => t.TicketStatus)
                                                            .Include(t => t.TicketType)
                                                            .Include(t => t.Project)
                                                     .Where(t => t.TicketPriorityId == TicketPriorityId)
                                                     .ToListAsync();

                return tickets;
            }
            catch (Exception)
            {
                throw;
            }

            

           


            
        }

        public async Task<List<Ticket>> GetAllTicketsByStatusAsync(int companyId, string statusName)
        {

            int ticketStatusId = (await LookupTicketStatusIdAsync(statusName)).Value;

            try
            {
                List<Ticket> tickets = await _context.Projects.Where(t => t.CompanyId == companyId)
                                                   .SelectMany(t => t.Tickets)
                                                       .Include(t => t.Attachments)
                                                       .Include(t => t.Comments)
                                                       .Include(t => t.DeveloperUser)
                                                       .Include(t => t.History)
                                                       .Include(t => t.OwnerUser)
                                                       .Include(t => t.TicketPriority)
                                                       .Include(t => t.TicketStatus)
                                                       .Include(t => t.TicketType)
                                                       .Include(t => t.Project)
                                            .Where(t => t.TicketStatusId == ticketStatusId)
                                            .ToListAsync();
                return tickets;
            }catch(Exception)
            {
                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByTypeAsync(int companyId, string typeName)
        {
            int ticketTypeId = (await LookupTicketTypeIdAsync(typeName)).Value;

            try
            {
                List<Ticket> tickets = await _context.Projects
                                                        .Where(p => p.CompanyId == companyId)
                                                        .SelectMany(p => p.Tickets)
                                                            .Include(t => t.Attachments)
                                                            .Include(t => t.Comments)
                                                            .Include(t => t.DeveloperUser)
                                                            .Include(t => t.History)
                                                            .Include(t => t.OwnerUser)
                                                            .Include(t => t.TicketPriority)
                                                            .Include(t => t.TicketStatus)
                                                            .Include(t => t.TicketType)
                                                            .Include(t => t.Project)
                                                        .Where(t => t.TicketTypeId == ticketTypeId)
                                                        .ToListAsync();

                return tickets;
            }
            catch (Exception)
            {
                throw;
            }
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

        public async Task<BTUser> GetTicketDeveloperAsync(int ticketId, int companyId)
        {
            BTUser developer = new();

            try
            {
                Ticket ticket = (await GetAllTicketsByCompanyAsync(companyId)).FirstOrDefault(t => t.Id == ticketId);

                if( ticket?.DeveloperUserId != null  )
                {
                    developer = ticket.DeveloperUser;
                }

                return developer;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<Ticket>> GetTicketsByRoleAsync(string role, string userId, int companyId)
        {
            List<Ticket> tickets = new ();

             try
            {
                if(role == Roles.Admin.ToString())
                {
                    tickets = await GetAllTicketsByCompanyAsync(companyId);

                }
                else if (role == Roles.Developer.ToString())
                {
                    tickets = (await GetAllTicketsByCompanyAsync(companyId)).Where( t => t.DeveloperUserId == userId).ToList();
                }
                else if (role == Roles.Submitter.ToString())
                {
                    tickets = (await GetAllTicketsByCompanyAsync(companyId)).Where(t => t.OwnerUserId == userId).ToList();
                }
                else if (role == Roles.ProjectManager.ToString())
                {
                    tickets = await GetTicketsByUserIdAsync(userId, companyId);
                }

                return tickets;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Ticket>> GetTicketsByUserIdAsync(string userId, int companyId)
        {

            BTUser user = await _context.Users.FirstOrDefaultAsync( u => u.Id == userId);
            List<Ticket> tickets = new();

            try
            {

                if( await _rolesService.IsUserInRoleAsync(user, Roles.Admin.ToString()) )
                {
                    tickets = ( await _projectService.GetAllProjectsByCompanyAsync(companyId) ).SelectMany( t => t.Tickets ).ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(user, Roles.Developer.ToString()))
                {
                    tickets = (await _projectService
                                      .GetAllProjectsByCompanyAsync(companyId))
                                      .SelectMany(t => t.Tickets)
                                      .Where( t => t.DeveloperUserId == userId).ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(user, Roles.Submitter.ToString()))
                {
                    tickets = (await _projectService
                                      .GetAllProjectsByCompanyAsync(companyId))
                                      .SelectMany(t => t.Tickets)
                                      .Where(t => t.OwnerUserId == userId).ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(user, Roles.ProjectManager.ToString()))
                {
                    tickets = (await _projectService.GetUserProjectsAsync(userId)).SelectMany(t => t.Tickets).ToList();
                }

                return tickets;
            }
            catch (Exception)
            {
                throw;
            }

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

        public async Task<int?> LookupTicketTypeIdAsync(string typeName)
        {
            try
            {
                TicketType ticketType = (await _context.TicketTypes.FirstOrDefaultAsync(t => t.Name == typeName));
                return ticketType?.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task RestoreTicketAsync(Ticket ticket)
        {
            throw new NotImplementedException();
        }

       
    }
}
