using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ICompanyRepository
    {
        PagedList<Company> GetAllCompanies(CompanyParameters companyParameters);

        Company GetCompany(Guid companyId);

        void CreateCompany(Company company);

        IEnumerable<Company> GetByIds(IEnumerable<Guid> ids);

        void DeleteCompany(Company company);
    }
}
