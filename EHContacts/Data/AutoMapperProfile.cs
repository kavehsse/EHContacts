using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EHContacts.Data.Entities;
using EHContacts.Models;


namespace EHContacts.Data
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<ContactViewModel, Contact>()
        .ForMember(o => o.User, ex => ex.Ignore())
        .ReverseMap();

      
    }
  }
}
