using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _repository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> GetAllCities()
        {
            try
            {
                var cityEntities = await _repository.GetCitiesAsync();
                // vanilla Mapping

                //var results = new List<CityWithoutPointOfInterestDto>();
                //foreach (var item in cityEntities)
                //{
                //    results.Add(new CityWithoutPointOfInterestDto
                //    {
                //        Id = item.Id,
                //        Name = item.Name,
                //        Description = item.Description,
                //    });
                //}
                var results = _mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cityEntities);
                return Ok(results);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            try
            {
                var city = await _repository.GetCityAsync(id, true);
                if(city == null)
                {
                    return NotFound("city is not found");
                }
                if (includePointsOfInterest)
                {
                    return Ok(_mapper.Map<CityDto>(city));
                }
                else
                {
                    return Ok(_mapper.Map<CityWithoutPointOfInterestDto>(city));
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "OOPS! there is a problem in the server");
            }


        }
    }

}
