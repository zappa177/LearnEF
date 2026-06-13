using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;


namespace LearnWebUI.Controllers
{

    [Route("person")]
    public class PersonController : Controller
    {
        //private field
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        public PersonController(IPersonService personService, ICountriesService countriesService)
        {
            _personService = personService;
            _countriesService = countriesService;
        }



        [Route("index")]
        [Route("/")]
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortby/* = nameof(PersonResponse.PersonName)*/,
            SortOrderOptions sortOrder /*= SortOrderOptions.ASC*/)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName) , "Person Name" },
                { nameof(PersonResponse.Email) , "Email" },
                { nameof(PersonResponse.DateOfBirth) , "DateOfBirth" },
                { nameof(PersonResponse.Gender) , "Gender" },
                { nameof(PersonResponse.Country) , "Country" },
                { nameof(PersonResponse.Address) , "Address" }
            };

            List<PersonResponse> persons = await _personService.GetFilteredPersons(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;


            //sort
            List<PersonResponse> sortedPersons = await _personService.GetSortedPersons(persons, sortby, sortOrder);
            ViewBag.CurrentSortBy = sortby;
            ViewBag.CurrentSortOrder = sortOrder.ToString();
            return View(sortedPersons);
        }
        [Route("create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countries = await _countriesService.GetAllCountries();

            ViewBag.Countries = countries.Select(country => new SelectListItem()
            {
                Text = country.CountryName,
                Value = country.CountryID.ToString()
            });
            return View();
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = await _countriesService.GetAllCountries();
                ViewBag.Countries = countries.Select(country => new SelectListItem()
                {
                    Text = country.CountryName,
                    Value = country.CountryID.ToString()
                });
                ViewBag.Error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            PersonResponse personResponse = await _personService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Person");
        }

        [HttpGet]
        [Route("edit/{personID:guid}")]
        public async Task<IActionResult> Edit(Guid personID)
        {
            PersonResponse? personResponse = await _personService.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(country => new SelectListItem()
            {
                Text = country.CountryName,
                Value = country.CountryID.ToString()
            });

            return View(personUpdateRequest);
        }
        [HttpPost]
        [Route("edit/{personID:guid}")]
        public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
        {

            PersonResponse? personResponse = await _personService.GetPersonByPersonID(personUpdateRequest.PersonID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                PersonResponse updatedPerson = await _personService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
            }
            else
            {
                List<CountryResponse> countries = await _countriesService.GetAllCountries();
                ViewBag.Countries = countries.Select(country => new SelectListItem()
                {
                    Text = country.CountryName,
                    Value = country.CountryID.ToString()
                });
                ViewBag.Error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
        }

        [HttpGet]
        [Route("delete/{personID:guid}")]
        public async Task<IActionResult> Delete(Guid personID)
        {
            PersonResponse? personResponse = await _personService.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            return View(personResponse);
        }

        [HttpPost]
        [Route("delete/{personID:guid}")]
        public async Task<IActionResult> DeleteConfirmed(PersonUpdateRequest person)
        {
            PersonResponse? personResponse = await _personService.GetPersonByPersonID(person.PersonID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }
            await _personService.DeletePerson(person.PersonID);
            return RedirectToAction("Index");
        }
        [Route("personspdf")]
        public async Task<IActionResult> PersonsPDF()
        {
            //get list person
            List<PersonResponse> persons = await _personService.GetAllPersons();


            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins(20, 20, 20, 20),
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,

            };
        }
    }
}
