using USModel;
using MSModel;
using MSRepositories;
using USRepositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace MSServices
{
    /// <summary>
    /// Service responsible for sending messages between users.
    ///
    /// Coordinates with an <see cref="IMessageContainer{Message,int}"/> to persist
    /// messages and with an <see cref="IUserDbRepo"/> to validate users (e.g. the
    /// receiver exists and the sender is not the same as the receiver).
    /// </summary>
    public sealed class MessageSenderService : IDisposable
    {
        private IMessageContainer<Message, int> _container;
        private IUserDbRepo _UserRepo;

        public MessageSenderService(IMessageContainer<Message, int> container, IUserDbRepo repo)
        {
            _container = container;
            _UserRepo = repo;
        }

        public bool Send(int SenderId, int ReceiverId, string MessageText)
        {
            if (_UserRepo.Search(ReceiverId) != null && ReceiverId != SenderId)
            {
                return _container.Add(ReceiverId, SenderId, MessageText);
            }

            return false;
        }

        public void Dispose()
        {

        }

    }
}
