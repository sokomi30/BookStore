using AutoMapper;
using BookStore.Application.DTOs;
using BookStore.Domain.Models;

namespace BookStore.Application.Mappings;

public class BookProfile : Profile
{
    public BookProfile()
    {
        // Маппинг для книг
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.AuthorFullName,
               opt => opt.MapFrom(src => src.Author != null ? src.Author.FullName : "Unknown"))
            .ForMember(dest => dest.AuthorId,
               opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(dest => dest.CoverImagePath, opt => opt.MapFrom(src => src.CoverImagePath));

        CreateMap<CreateBookDto, Book>();

        // Маппинг для авторов (простой, без DTO)
        CreateMap<Author, AuthorSimpleDto>();
    }
}