namespace WorkFlow360.Domain.Common
{
    public abstract class Entity
    {
        protected Entity(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException(
                    "Entity id cannot be empty.",
                    nameof(id));
            }

            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
