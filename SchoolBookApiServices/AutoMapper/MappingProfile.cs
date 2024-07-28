
namespace SchoolBookShop.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile() 
        { 
            CreateMap<RegisterDto,ApplicationUser>().ReverseMap();
            CreateMap<AddBookDto, Book>().ReverseMap();
            CreateMap<Book, BookDto>().ReverseMap();
            //CreateMap<UpdateDto,Book>().ReverseMap();
            //CreateMap<CartDto,Cart>().ReverseMap();
            //CreateMap<CartItem,CartItemDto>().ReverseMap();
            //CreateMap<Order,OrderDto>().ReverseMap();
            //CreateMap<OrderItem,OrderItemDto>().ReverseMap();
        }

    }
}
