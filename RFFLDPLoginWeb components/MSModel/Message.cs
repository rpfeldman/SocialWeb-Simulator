using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MSModel
{
    public class Message
    {
        private int _MessageId;
        private int _SenderId;
        private int _ReceiverId;
        private string _Message = string.Empty;

        [Key]
        public int MessageId { get { return _MessageId; } set { _MessageId = value; } }
        public int SenderId { get { return _SenderId; } set { _SenderId = value; } }
        public int ReceiverId { get { return _ReceiverId; } set { _ReceiverId = value; } }
        public string MessageText
        {
            get { return _Message; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Invalid message text");
                }
                _Message = value;
            }
        }
    }
}

