using AutoMapper;
using GroceryStoreData.Models;
using MyfirstLib.Models;

namespace MyfirstLib.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductModel>()
                .ReverseMap()
                .ForMember(x => x.StoreProducts, o => o.Ignore());
            CreateMap<ShoppingList, ShoppingListModel>()
                .ForMember(x => x.UserName, o => o.MapFrom(s => s.User.Email))
                .ForMember(x => x.Store, o => o.MapFrom(s => s.Store.Name))
                .ReverseMap()
                .ForMember(x => x.User, o => o.Ignore())
                .ForMember(x => x.Store, o => o.Ignore());
            CreateMap<ShoppingListItem, ShoppingListItemModel>()
                .ForMember(x => x.ShoppingList, o => o.MapFrom(s => s.ShoppingList.Title))
                .ForMember(x => x.Product, o => o.MapFrom(s => s.StoreProduct.Product.Name))
                .ForMember(x => x.UnitOfMeasure, o => o.MapFrom(s => s.StoreProduct.Product.UnitOfMeasure))
                .ReverseMap()
                .ForMember(x => x.ShoppingList, o => o.Ignore())
                .ForMember(x => x.StoreProduct, o => o.Ignore())                ;
            CreateMap<State, StateModel>()
                .ReverseMap();
            CreateMap<Store, StoreModel>()
                .ForMember(x => x.State, o => o.MapFrom(s => s.State.Name))
                .ReverseMap()
                .ForMember(x => x.State, o => o.Ignore());
            CreateMap<StoreProduct, StoreProductModel>()
                .ForMember(x => x.Store, o => o.MapFrom(s => s.Store.Name))
                .ForMember(x => x.Product, o => o.MapFrom(s => s.Product.Name))
                .ReverseMap()
                .ForMember(x => x.Store, o => o.Ignore())
                .ForMember(x => x.Product, o => o.Ignore());
            CreateMap<User, UserModel>()
                .ReverseMap();
            CreateMap<UvShoppingList, UvShoppingListModel>()
                .ReverseMap();
        }
    }
}
