using System;
using FinanceAccounting.Domain.Entities.Base;

namespace FinanceAccounting.BusinessLogic.Common.Models
{
    public class RefreshToken : IEntity<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
