using System;
using System.Collections.Generic;
using System.Text;

namespace MSRepositories
{
    public interface IMessageContainer<TMessage, TIdentifier>
    {
        public List<TMessage> SearchReceiverMessages(TIdentifier ReceiverId);

        public TMessage? Search(TIdentifier identifier);
        public bool Add(TIdentifier ReceiverId, TIdentifier SenderId, string MessageText);
        public bool Remove(TIdentifier identifier);
    }
}
