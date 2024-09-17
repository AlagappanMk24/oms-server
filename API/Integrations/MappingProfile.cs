using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Integrations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Invoice, InvoiceDto>().ReverseMap();
            CreateMap<InvoiceItem, InvoiceItemDto>().ReverseMap();
            CreateMap<InvoiceAttachments, InvoiceAttachmentsDto>().ReverseMap();    
        }
    }
}
