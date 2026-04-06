using MSModel;
using MSRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSServices
{
    public sealed class UserMessagesService : IDisposable
    {
        private IMessageContainer<Message, int> _container;
        private int _UserId;
        public UserMessagesService(IMessageContainer<Message, int> container, int UserId)
        {
            _container = container;
            _UserId = UserId;
        }
        
        public List<Message> GetUserMessages()
        {
            return _container.SearchReceiverMessages(_UserId);
        }

        public void MarkAsRead(int MessageId)
        {
            _container.Remove(MessageId);
        }
        public void MarkAllAsRead()
        {
            foreach (var item in GetUserMessages())
            {
                _container.Remove((item.MessageId));
            }
        }
        public void Dispose()
        {
        }
    }
}
