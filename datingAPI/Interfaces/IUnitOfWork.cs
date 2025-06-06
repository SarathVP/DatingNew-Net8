namespace datingAPI.Interfaces
{
    public interface IUnitOfWork
    {
        ILikesRepository LikesRepository { get; }
        IMessageRepository MessageRepository { get; }
        IUserRepository UserRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}