using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MSModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSRepositories
{
    /// <summary>
    /// Entity Framework implementation of a message repository for the <see cref="Message"/> entity.
    ///
    /// This class provides persistence operations over the database table represented by
    /// <see cref="MessageContainerDbContext.MessageContainer"/> and implements the
    /// <see cref="IMessageContainer{Message,int}"/> interface.
    /// </summary>
    public sealed class EF_MessageContainer : IMessageContainer<Message, int>
    {
        private MessageContainerDbContext _context;
        public EF_MessageContainer(MessageContainerDbContext context)
        {
            _context = context;
        }

        private int IdSetter()
        {
            int IdCount;

            IdCount = _context.MessageContainer.Count();

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

                MessageText = MessageText.Length < _context.MessageCharLimit ? MessageText : throw new Exception("MessageText characters must be less than 150");

                var _message = new Message() { MessageId = IdSetter(), SenderId = SenderId, ReceiverId = ReceiverId, MessageText = MessageText };
                _context.MessageContainer.Add(_message);
                _context.SaveChanges();
               
                return true;
            }
            catch (Exception)
            {
                return false;
            }
           
        }

        public bool Remove(int MessageId)
        {
            try
            {
                var message = _context.MessageContainer.Where(m => m.MessageId == MessageId).FirstOrDefault();
                _context.MessageContainer.Remove(message ?? throw new Exception("non-existent message"));
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Message> SearchReceiverMessages(int ReceiverId)
        {
            return _context.MessageContainer.Where(m => m.ReceiverId == ReceiverId).ToList();
        }

        public Message? Search(int identifier)
        {
            return _context.MessageContainer.Where(m => m.MessageId == identifier).FirstOrDefault();
        }
    }

    public class MessageContainerDbContext : DbContext
    {
        public readonly int MessageCharLimit;
        public MessageContainerDbContext(DbContextOptions options, int ColumnCharLimit) : base(options)
        {
            MessageCharLimit = ColumnCharLimit;
        }

        public DbSet<Message> MessageContainer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Message>().Property(m => m.MessageId).ValueGeneratedNever();
            modelBuilder.Entity<Message>().Property(m => m.MessageId).HasColumnName<int>("MessageId");
            modelBuilder.Entity<Message>().HasKey(m => m.MessageId);
        }
    }
}
