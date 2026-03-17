using AutoMapper;
using Library.API.Dtos.BookCopyDtos;
using Library.API.Dtos.BookDtos;
using Library.API.Dtos.BorrowingRecordDtos;
using Library.API.Dtos.FineDtos;
using Library.API.Dtos.GenreDtos;
using Library.API.Dtos.UserDtos;
using Library.Models.Models;

namespace Library.API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<BookCreateDto, Book>().ReverseMap();
            CreateMap<BookUpdateDto, Book>().ReverseMap();
            CreateMap<Genre, GenreDto>().ReverseMap();
            CreateMap<BookCopy, CopyDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserCreateDto, User>().ForMember(dest => dest.PasswordHash, opt => opt.Ignore());  
            CreateMap<RegisterDto, User>().ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<UserUpdateDto, User>().ReverseMap();
            CreateMap<Fine, FineDto>().ReverseMap();
            CreateMap<BorrowBookDto, BorrowingRecord>().ReverseMap();
            CreateMap<BorrowingRecord, BorrowingRecordDto>().ReverseMap();
        }
    }
}
