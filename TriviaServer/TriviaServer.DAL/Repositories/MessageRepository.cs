using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaServer.Models;
using TriviaServer.Models.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TriviaServer.DAL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly TriviaRankContext _context;
        public MessageRepository(TriviaRankContext context)
        {
            _context = context;
        }

        public async Task<int> createMessage(int senderId, int receiverId, string body)
        {
            if (await validateSenderReceiver(senderId, receiverId))
            {
                throw new InvalidOperationException("invalid client operation");
            }

            Message message = new Message()
            {
                FromId = senderId,
                ToId = receiverId,
                Body = body,
                Date = DateTimeOffset.Now
            };
            await _context.AddAsync(message);
            await saveAsync();

            return message.Id;
        }

        public async Task<IEnumerable<MessageModel>> getLastNMessages(int senderId, int receiverId, int numMessages)
        {
            if (await validateSenderReceiver(senderId, receiverId) || !Enumerable.Range(1, 50).Contains(numMessages))
            {
                throw new InvalidOperationException("invalid client operation");
            }

            List<MessageModel> messages = await _context.Messages.Include(m => m.From).Include(m => m.From)
                .Where(m => m.FromId == senderId && m.ToId == receiverId)
                .Select(m => new MessageModel()
                {
                    Id = m.Id,
                    senderUsername = m.From.Username,
                    receiverUsername = m.To.Username,
                    FromId = m.FromId,
                    ToId = m.ToId,
                    Body = m.Body,
                    Date = m.Date
                })
                .OrderBy(m => m.Date)
                .ToListAsync();

            return messages.TakeLast(numMessages);
        }

        public async Task<List<MessageModel>> getMessagesBetweenDate(int senderId, int receiverId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            if (await validateSenderReceiver(senderId, receiverId))
            {
                throw new InvalidOperationException("invalid client operation");
            }

            List<MessageModel> messages = await _context.Messages.Include(m => m.From).Include(m => m.From)
                .Where(m => m.FromId == senderId && m.ToId == receiverId && (m.Date >= startDate && m.Date <= endDate))
                .Select(m => new MessageModel()
                {
                    Id = m.Id,
                    senderUsername = m.From.Username,
                    receiverUsername = m.To.Username,
                    FromId = m.FromId,
                    ToId = m.ToId,
                    Body = m.Body,
                    Date = m.Date
                })
                .OrderBy(m => m.Date)
                .ToListAsync();

            return messages;
        }

        public async Task saveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> validateSenderReceiver(int fromId, int toId)
        {
            bool invalidSender = await _context.Players.FindAsync(fromId) == null;
            bool invalidReceiver = await _context.Players.FindAsync(toId) == null;
            return invalidSender || invalidReceiver;
        }
    }
}
