using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PiServices.API.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public CompaniesController(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetCompanies([FromQuery] CompanyParameters companyParameters)
        {
            try
            {
                var companies = _repository.Company.GetAllCompanies(companyParameters);

                return Ok(companies);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong in the {nameof(GetCompanies)} action {ex}");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name ="CompanyById")]
        public IActionResult GetCompany(Guid id)
        {
            var company = _repository.Company.GetCompany(id);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database");
                return NotFound();
            }
            else
            {
                return Ok(company);
            }
        }

        [HttpPost]
        public IActionResult CreateCompany([FromBody]Company company)
        {
            if (company == null)
            {
                _logger.LogError("Company object sent from client is null.");
                return BadRequest("Company object is null");
            }

            _repository.Company.CreateCompany(company);
            _repository.Save();

            return CreatedAtRoute("CompanyById", new { id = company.Id }, company);
        }

        //[HttpGet("collection/{ids}", Name ="CompanyCollection")]
        //public IActionResult GetCompanyCollection(IEnumerable<Guid> ids)
        //{
        //    if (ids == null)
        //    {
        //        _logger.LogError("Parameter ids is null");
        //        return BadRequest("Parameter ids is null");
        //    }

        //    var companyEntities = _repository.Company.GetByIds(ids, trackChanges: false);

        //    if(ids.Count() != companyEntities.Count())
        //    {
        //        _logger.LogError("Some ids are not valid in a collection");
        //        return NotFound();
        //    }

        //    var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

        //    return Ok(companiesToReturn);
        //}

        [HttpDelete("{id}")]
        public IActionResult DeleteCompany(Guid id)
        {
            var company = _repository.Company.GetCompany(id);
            if(company == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _repository.Company.DeleteCompany(company);
            _repository.Save();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCompany(Guid id, [FromBody] Company company)
        {
            if (company == null)
            {
                _logger.LogError("Company object sent from client is null.");
                return BadRequest("Company object is null");
            }

            var companyEntity = _repository.Company.GetCompany(id);
            if(companyEntity == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            companyEntity.Name = company.Name;
            companyEntity.Address = company.Address;
            companyEntity.Country = company.Country;

            _repository.Save();

            return Ok(companyEntity);
        }
    }
}
