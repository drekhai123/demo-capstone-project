using EduSource.Domain.Abstraction.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSource.Domain.Entities
{
    public class Seller : DomainEntity<Guid>
    {
        public Seller()
        {

        }

        public string QRUrl { get; private set; }
        public double Rating { get; private set; }
        public Guid AccountId { get; private set; }
        public virtual Account Account { get; private set; }
    }
}
