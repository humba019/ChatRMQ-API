using AutoMapper;
using ChatProducer.Domain.Models;
using ChatProducer.Resources.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Chat, ChatResource>();
            CreateMap<Client, ClientResource>();
            CreateMap<Message, MessageResource>();
            CreateMap<Token, TokenResource>();
        }
    }
}
