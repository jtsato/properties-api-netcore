namespace Infra.Firestore.Commons.Repository;

public abstract class Entity: IEntity
{
    public string Id { get; set; }
}