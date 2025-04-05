using EduSource.Contract.Enumarations.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSource.Contract.DTOs.Account
{
    public static class GetUserByYearAndMonthDTO
    {
        public class CustomerDTO
        {
            public string Email { get; set; } = string.Empty;
            public RoleType RoleId { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        public class MonthDTO
        {
            public int MonthOfYear { get; set; }
            public int CustomersCount { get; set; }
            public List<CustomerDTO> Customers { get; set; }
        }
    }
}
