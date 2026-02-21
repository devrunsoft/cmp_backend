namespace ScoutDirect.Core.Entities.Base
{
    public interface IEntity
    {
    }

    public interface IIdentityObject<TPrimaryKey> : IEntity
    {
       TPrimaryKey Id { get; set; }

       DateTime? IsDelete { get; set; }
    }
 
    public abstract class IPersistentObject<TPrimaryKey> : IIdentityObject<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }
        public virtual DateTime? IsDelete { get; set; }
    }
}
