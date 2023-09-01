﻿using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using TheBugTracker.Data;
using TheBugTracker.Models;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Services
{
    public class BTNotificationService : IBTNotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IBTRolesService _rolesService;

        public BTNotificationService(ApplicationDbContext context, IEmailSender emailSender, IBTRolesService rolesService)
        {
            _context = context;
            _emailSender = emailSender;
            _rolesService = rolesService;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            try
            {
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Notification>> GetReceivedNotificationsAsync(string userId)
        {
            try
            {

                List<Notification> notifications = await _context.Notifications
                                                                    .Include(n => n.Recipient)
                                                                    .Include(n => n.Sender)
                                                                    .Include(n => n.Ticket)
                                                                        .ThenInclude(t => t.Project)
                                                            .Where(t => t.RecipientId == userId).ToListAsync();

                return notifications;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Notification>> GetSentNotificationsAsync(string userId)
        {
            try
            {

                List<Notification> notifications = await _context.Notifications
                                                                    .Include(n => n.Recipient)
                                                                    .Include(n => n.Sender)
                                                                    .Include(n => n.Ticket)
                                                                        .ThenInclude(t => t.Project)
                                                            .Where(t => t.SenderId == userId).ToListAsync();

                return notifications;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SendEmailNotificationAsync(Notification notification, string emailSubject)
        {
            BTUser Btuser = await _context.Users.FirstOrDefaultAsync( u => u.Id == notification.RecipientId);

            if(Btuser != null)
            {
                string userEmail = Btuser.Email;
                string message = notification.Message;

                // Send email
                try
                {
                    await _emailSender.SendEmailAsync(userEmail, emailSubject, message);
                    return true;
                }
                catch (Exception)
                {
                    throw;
                }

            }
            else
            {
                return false;
            }

        }

        public Task SendEmailNotificationsByRoleAsync(Notification notification, int companyId, string role)
        {
            throw new NotImplementedException();
        }

        public Task SendMembersEmailNotificationsAsync(Notification notification, List<BTUser> members)
        {
            throw new NotImplementedException();
        }
    }
}
