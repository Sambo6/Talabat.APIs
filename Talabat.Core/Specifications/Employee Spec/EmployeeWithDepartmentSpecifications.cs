using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Employee_Spec
{
    public class EmployeeWithDepartmentSpecifications : BaseSpecifications<Employee>
    {
        public EmployeeWithDepartmentSpecifications()
            : base()
        {
            AddIncludes();
        }
        public EmployeeWithDepartmentSpecifications(int id)
            : base(E => E.Id == id)
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            Includes.Add(E => E.Department);
        }
    }
}
