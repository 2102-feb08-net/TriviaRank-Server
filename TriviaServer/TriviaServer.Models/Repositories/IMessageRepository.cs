using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaServer.Models.Repositories
{
    public interface IMessageRepository
    {
        public Task<IEnumerable<MessageModel>> getLastNMessages(int senderId, int receiverId, int numMessages);
        public Task<List<MessageModel>> getMessagesBetweenDate(int senderId, int receiverId, DateTimeOffset startDate, DateTimeOffset endDate);
        public Task<int> createMessage(int senderId, int receiverId, string body);
        public Task saveAsync();
    }
}
