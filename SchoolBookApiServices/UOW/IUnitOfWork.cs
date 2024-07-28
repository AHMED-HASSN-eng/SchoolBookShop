namespace SchoolBookShop.UOW
{
    public interface IUnitOfWork
    {
        //IAccountRepository AccountRepository { get; }
        //IBookRepository BookRepository { get; }
        //ICartItemRepository CartItemRepository { get; }
        //ICartRepository CartRepository { get; }
        //IOrderItemRepository OrderItemRepository { get; }
        //IOrderRepository OrderRepository { get; }
        Task<int> Save();
    }
}
