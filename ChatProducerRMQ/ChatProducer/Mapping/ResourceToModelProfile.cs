using AutoMapper;
using ChatProducer.Domain.Models;
using ChatProducer.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<SaveChatResource, Chat>();
            CreateMap<SaveClientResource, Client>();
            CreateMap<SaveMessageResource, Message>();
            CreateMap<SaveTokenResource, Token>();

        }
    }
}
