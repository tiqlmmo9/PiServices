using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public PagedList<Company> GetAllCompanies(CompanyParameters companyParameters)
        {
            var companies = FindAll().Search(companyParameters.SearchTerm).OrderBy(c => c.Name).ToList();

            return PagedList<Company>.ToPagedList(companies, companyParameters.PageNumber, companyParameters.PageSize);
        }

        public Company GetCompany(Guid companyId) => FindByCondition(c => c.Id.Equals(companyId)).SingleOrDefault();


        public void CreateCompany(Company company) => Create(company);

        public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids) => FindByCondition(x => ids.Contains(x.Id)).ToList();

        public void DeleteCompany(Company company)
        {
            Delete(company);
        }
    }
}
