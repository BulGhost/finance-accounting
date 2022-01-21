using System.Collections.Generic;
using FinanceAccounting.Domain.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace FinanceAccounting.Domain.Entities
{
    public class User : IdentityUser<int>, IEntity<int>
    {
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Operation> Operations { get; set; } = new List<Operation>();
    }
}
