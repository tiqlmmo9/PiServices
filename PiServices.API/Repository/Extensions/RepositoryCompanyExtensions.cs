using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Extensions
{
    public static class RepositoryCompanyExtensions
    {
        public static IQueryable<Company> Search(this IQueryable<Company> companies, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return companies;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return companies.Where(c => c.Name.ToLower().Contains(lowerCaseTerm));
        }
    }
}
