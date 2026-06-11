using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Index(string searchBy, string? searchString, string sortby/* = nameof(PersonResponse.PersonName)*/,
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

            List<PersonResponse> persons = _personService.GetFilteredPersons(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;


            //sort
            List<PersonResponse> sortedPersons = _personService.GetSortedPersons(persons, sortby, sortOrder);
            ViewBag.CurrentSortBy = sortby;
            ViewBag.CurrentSortOrder = sortOrder.ToString();
            return View(sortedPersons);
        }
        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            List<CountryResponse> countries = _countriesService.GetAllCountries();

            ViewBag.Countries = countries.Select(country => new SelectListItem()
            {
                Text = country.CountryName,
                Value = country.CountryID.ToString()
            });
            return View();
        }
        [HttpPost]
        [Route("create")]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries.Select(country => new SelectListItem()
                {
                    Text = country.CountryName,
                    Value = country.CountryID.ToString()
                });
                ViewBag.Error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Person");
        }

        [HttpGet]
        [Route("edit/{personID:guid}")]
        public IActionResult Edit(Guid personID)
        {
            PersonResponse personResponse = _personService.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(country => new SelectListItem()
            {
                Text = country.CountryName,
                Value = country.CountryID.ToString()
            });

            return View(personUpdateRequest);
        }
        [HttpPost]
        [Route("edit/{personID:guid}")]
        public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
        {

            PersonResponse? personResponse = _personService.GetPersonByPersonID(personUpdateRequest.PersonID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                PersonResponse updatedPerson = _personService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
            }
            else
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
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
        public IActionResult Delete(Guid personID)
        {
            PersonResponse? personResponse = _personService.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            return View(personResponse);
        }

        [HttpPost]
        [Route("delete/{personID:guid}")]
        public IActionResult DeleteConfirmed(PersonUpdateRequest person)
        {
            PersonResponse? personResponse = _personService.GetPersonByPersonID(person.PersonID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }
            _personService.DeletePerson(person.PersonID);
            return RedirectToAction("Index");
        }
    }
}
