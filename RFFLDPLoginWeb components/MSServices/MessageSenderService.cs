using Model;
using MSModel;
using MSRepositories;
using Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace MSServices
{
    public class MessageSenderService : IDisposable
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
