namespace API.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IMessageRepository MessageRepository { get; }
    ILikesRepository LikesRepository { get; }
    IVisitsRepository VisitsRepository { get; }
    IPhotoRepository PhotoRepository {get;}
    Task<bool> Complete();
    bool HasChanges();
}