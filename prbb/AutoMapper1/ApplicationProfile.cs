using AutoMapper;
using ModelsLayer.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsLayer.AutoMapper
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<MyClubModelDTO, MyClubModel>()
                .ReverseMap();
            CreateMap<CreateEventModelDTO, CreateEventModel>()
                .ReverseMap();
            CreateMap<EditEventsModelDTO, EditEventsModel>()
                .ReverseMap();
        }
    }
}
