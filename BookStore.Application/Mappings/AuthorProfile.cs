using AutoMapper;
using BookStore.Domain.Models;
using BookStore.Application.DTOs;

namespace BookStore.Application.Mappings
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>();
            CreateMap<CreateAuthorDto, Author>();
            CreateMap<UpdateAuthorDto, Author>();
        }
    }
}