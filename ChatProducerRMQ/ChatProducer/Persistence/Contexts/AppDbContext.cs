
using ChatProducer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Chat> Chat { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Message> Message { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Client>().ToTable("CLIENT");
            builder.Entity<Client>().HasKey(a => a.ClientEmail);
            builder.Entity<Client>().Property(a => a.ClientEmail).IsRequired();
            builder.Entity<Client>().Property(a => a.ClientName).IsRequired();

            builder.Entity<Client>().HasData(
                new Client { ClientName = "Humberto", ClientEmail = "humba01@email", ClientPass = "12345" },
                new Client { ClientName = "Doisberto", ClientEmail = "humba02@email", ClientPass = "12345" }
            );


            builder.Entity<Chat>().ToTable("CHAT");
            builder.Entity<Chat>().HasKey(a => a.ChatId);
            builder.Entity<Chat>().Property(a => a.ChatId).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Chat>().Property(a => a.From).IsRequired();
            builder.Entity<Chat>().Property(a => a.To).IsRequired();

            builder.Entity<Chat>().HasData(
               new Chat { ChatId = 1, From = "humba02@email", To = "humba01@email" },
               new Chat { ChatId = 2, From = "humba01@email", To = "humba02@email" }
            );


            builder.Entity<Message>().ToTable("MESSAGE");
            builder.Entity<Message>().HasKey(a => a.MessageId);
            builder.Entity<Message>().Property(a => a.MessageId).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Message>().Property(a => a.MessageContent).IsRequired();
            builder.Entity<Message>().Property(a => a.ChatId).IsRequired();

            builder.Entity<Message>().HasData(
               new Message { MessageId = 1, MessageContent = "Olá boa tarde!", ChatId = 1 }
            );

        }
    }
}
