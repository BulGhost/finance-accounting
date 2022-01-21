namespace FinanceAccounting.Domain.Entities.Base
{
    public abstract class BaseEntity : IEntity<int>
    {
        public int Id { get; set; }
    }
}
