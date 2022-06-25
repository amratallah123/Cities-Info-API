using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/[controller]")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _localMailService;
        private readonly ICityInfoRepository _repository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService localMailService, ICityInfoRepository repository, IMapper mapper )
        {
            _logger = logger 
                ?? throw new ArgumentNullException(nameof(logger));
            _localMailService = localMailService;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            try
            {
                
                if(!await _repository.CityExistsAsync(cityId))
                {
                    return NotFound("this city is not found");
                }
                var pointsOfInterestForCity = await _repository.GetPointsOfInterestAsync(cityId);
                return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity)); 

            }
            catch (Exception exp)
            { 
                _logger.LogInformation($"Exeption while getting points of intersets for city with id {cityId}",exp);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "OOPS! there is a problem in the server");
            }

        }
        [HttpGet("{pointOfinterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            try
            {
                if (!await _repository.CityExistsAsync(cityId))
                {
                    return NotFound("this city is not found");
                }
                var pointOfInterestForCity = await _repository.GetPointOfInterestAsync(cityId, pointOfInterestId);
                if(pointOfInterestForCity == null)
                {
                    return NotFound("this point not found");
                }
                return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterestForCity));

            }
            catch (Exception exp)
            {
                _logger.LogCritical($"Exeption while getting point of interset for city with id {cityId}", exp);

                return this.StatusCode(StatusCodes.Status500InternalServerError, "OOPS! there is a problem in the server");
            }

        }
        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
           int cityId,
           PointOfInterestForCreationDto pointOfInterest)
        {
            if (!await _repository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            await _repository.AddPointOfInterestForCityAsync(
                cityId, finalPointOfInterest);

            await _repository.SaveChangesAsync();

            var createdPointOfInterestToReturn =
                _mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                 new
                 {
                     cityId = cityId,
                     pointOfInterestId = createdPointOfInterestToReturn.Id
                 },
                 createdPointOfInterestToReturn);
        }
        [HttpPut("{pointofinterestid}")]
        public async Task<ActionResult<PointOfInterestDto>> UpdatePointOfInterest(int cityId, int pointOfInterestId, 
            PointOfInterestForUpdateDto pointOfInterest)
        {
            try
            {
                if (!await _repository.CityExistsAsync(cityId))
                {
                    return NotFound("this city is not found");
                }
                var pointOfInterestEntity = await _repository.GetPointOfInterestAsync(cityId, pointOfInterestId);

                if (pointOfInterestEntity == null)
                {
                    return NotFound("this point is not found");
                }
               var finalPointOfInterest =  _mapper.Map(pointOfInterest, pointOfInterestEntity);

                await _repository.SaveChangesAsync();

                var createdPointOfInterestToReturn =
                _mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);

                return AcceptedAtRoute("GetPointOfInterest",
                    new
                    {
                        cityId = cityId,
                        pointOfInterestId = createdPointOfInterestToReturn.Id
                    }, createdPointOfInterestToReturn);

        
            }
            catch (Exception exp)
            {
                _logger.LogCritical($"Exeption while getting point of interset for city with id {cityId}", exp);

                return this.StatusCode(StatusCodes.Status500InternalServerError, "OOPS! there is a problem in the server");
            }
        }
        [HttpPatch("{pointofinterestid}")]
        public async Task<ActionResult<PointOfInterestDto>> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            try
            {
                if (!await _repository.CityExistsAsync(cityId))
                {
                    return NotFound("this city is not found");
                }
                var pointOfInterestEntity = await _repository.GetPointOfInterestAsync(cityId, pointOfInterestId);
                if (pointOfInterestEntity == null)
                {
                    return NotFound("this point is not found");
                }

                var pointOfInterestToPatch = _mapper.Map<Models.PointOfInterestForUpdateDto>(pointOfInterestEntity);

                patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);



                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (!TryValidateModel(pointOfInterestToPatch))
                {
                    BadRequest(ModelState);
                }
                _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
                await _repository.SaveChangesAsync();

                return AcceptedAtRoute("GetPointOfInterest",
                      new
                      {
                          cityId = cityId,
                          pointOfInterestId = pointOfInterestEntity.Id
                      }, pointOfInterestToPatch);

            }
            catch (Exception exp)
            {
                _logger.LogCritical($"Exeption while getting point of interset for city with id {cityId}", exp);

                return this.StatusCode(StatusCodes.Status500InternalServerError, "OOPS! there is a problem in the server");
            }
            return Ok();
        }
        [HttpDelete("{pointofinterestid}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            try
            {
                if (!await _repository.CityExistsAsync(cityId))
                {
                    return NotFound("this city is not found");
                }
                var pointOfInterestEntity = await _repository.GetPointOfInterestAsync(cityId, pointOfInterestId);
                if (pointOfInterestEntity == null)
                {
                    return NotFound("this point is not found");
                }

                _repository.DeletePointOfInterest(pointOfInterestEntity);

                await _repository.SaveChangesAsync();


            }
            catch (Exception exp)
            {
                _logger.LogCritical($"Exeption while getting point of interset for city with id {cityId}", exp);

                return this.StatusCode(StatusCodes.Status500InternalServerError, "OOPS! there is a problem in the server");
            }
            return Ok();
        }


    }
}
