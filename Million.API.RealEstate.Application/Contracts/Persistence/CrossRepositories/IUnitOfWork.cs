namespace Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;

        IPropertyRepository PropertyRepository {  get; }
        IPropertyTraceRepository PropertyTraceRepository {  get; }
    }
}