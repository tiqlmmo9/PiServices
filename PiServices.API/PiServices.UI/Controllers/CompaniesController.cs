using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PiServices.UI.Helper;
using PiServices.UI.Models;
using PiServices.UI.RequestFeatures;

namespace PiServices.UI.Controllers
{
    public class CompaniesController : Controller
    {
        PiServicesAPI _api = new PiServicesAPI();
        public async Task<IActionResult> Index([FromQuery] CompanyParameters companyParameters)
        {
            ViewData["SearchTerm"] = companyParameters.SearchTerm;
            List<Company> companies = new List<Company>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/companies?searchTerm={companyParameters.SearchTerm}");

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                companies = JsonConvert.DeserializeObject<List<Company>>(results);
            }

            var companiesToReturn = PagedList<Company>.ToPagedList(companies, companyParameters.PageNumber, companyParameters.PageSize);

            return View(companiesToReturn);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Company company)
        {
            HttpClient client = _api.Initial();

            //HTTP POST
            var postTask = client.PostAsJsonAsync<Company>("api/companies", company);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            Company company = null;
            HttpClient client = _api.Initial();

            //HTTP POST
            var res= await client.GetAsync($"api/companies/{id}");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;

                company = JsonConvert.DeserializeObject<Company>(result);
            }

            return View(company);
        }

        [HttpPost]
        public IActionResult Edit(Company company)
        {
            HttpClient client = _api.Initial();

            var putTask = client.PutAsJsonAsync<Company>($"api/companies/{company.Id}", company);
            putTask.Wait();

            var result = putTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            HttpClient client = _api.Initial();
            await client.DeleteAsync($"api/companies/{id}");

            return RedirectToAction("Index");
        }
    }
}
