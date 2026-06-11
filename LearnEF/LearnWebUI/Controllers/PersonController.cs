using Microsoft.AspNetCore.Mvc;
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
            ViewBag.Countries = countries;
            return View();
        }
        [HttpPost]
        [Route("create")]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;
                ViewBag.Error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Person");
        }
    }
}
