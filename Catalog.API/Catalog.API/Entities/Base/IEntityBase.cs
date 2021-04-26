namespace Catalog.API.Entities.Base
{
    public interface IEntityBase<T>
    {
        T Id { get; }
    }
}
