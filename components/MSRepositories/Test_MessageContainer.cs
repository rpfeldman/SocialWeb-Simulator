using MSModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSRepositories
{
    public class Test_MessageContainer : IMessageContainer<Message, int>
    {
        private List<Message> Container;

        public Test_MessageContainer()
        {
            Container = new List<Message>();
        }

        private int IdSetter()
        {
            int IdCount = Container.Count;

            while (Search(IdCount) != null)
            {
                IdCount++;
            }

            return IdCount;
        }


        public bool Add(int ReceiverId, int SenderId, string MessageText)
        {
            try
            {
                var Message = new Message() { MessageId = IdSetter(), ReceiverId = ReceiverId, SenderId = SenderId, MessageText = MessageText };
                Container.Add(Message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Remove(int identifier)
        {
            try
            {
                var Message = Container.Where(m => m.MessageId == identifier).FirstOrDefault() ?? throw new Exception("Non-existent message");
                Container.Remove(Message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Message? Search(int identifier)
        {
            return Container.Where(m => m.MessageId == identifier).FirstOrDefault();
        }

        public List<Message> SearchReceiverMessages(int ReceiverId)
        {
            return Container.Where(m => m.ReceiverId == ReceiverId).ToList();
        }
    }
}
