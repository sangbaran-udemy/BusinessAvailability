using AutoMapper;
using BusinessAvailability.API.Models.DTO;
using BusinessAvailability.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BusinessAvailability.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessServiceController : ControllerBase
    {
        private readonly IBusinessRepository businessRepository;
        private readonly IMapper mapper;
        private readonly ILogger<BusinessServiceController> logger;

        public BusinessServiceController(IBusinessRepository businessRepository, IMapper mapper, ILogger<BusinessServiceController> logger)
        {
            this.businessRepository = businessRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllBusinesses()
        {
            // Get the models from the repository..
            var businesses = businessRepository.GetAll();

            if (businesses.Count() == 0)
                return NotFound();

            // Convert the domain object to a DTO..
            var businessesDTO = mapper.Map<IEnumerable<BusinessServiceDTO>>(businesses);
            logger.Log(LogLevel.Information, $"Finished invoking GetAllBusinesses()..");

            // Return the DTO back to the client.. 
            return Ok(businessesDTO);
        }

        [HttpGet]
        [Route("{openTime}/{closeTime}")]
        public IActionResult GetAvailableBusinesses([FromRoute] [Required, MaxLength(5)] string openTime, [FromRoute] [Required, MaxLength(5)] string closeTime)
        {
            // Validate the input fields..
            var inputFields = new List<string> { openTime, closeTime};
            bool isValid = ValidateInputFields(inputFields);

            if(!isValid)
                return BadRequest("Please enter the values in HH:mm format.");
            
            // Get the domain models from the repository..
            var businesses = businessRepository.GetAvailableBusinesses(openTime, closeTime);

            if (businesses.Count() == 0)
                return NotFound();

            // Convert the domain models to a DTO..
            var businessesDTO = mapper.Map<IEnumerable<BusinessServiceDTO>>(businesses);
            logger.Log(LogLevel.Information, $"Finished invoking GetAvailableBusinesses() for time range: {0} & {1}", openTime, closeTime);

            // Return the DTO back to the client.. 
            return Ok(businessesDTO);
        }

        private bool ValidateInputFields(List<string> inputFields)
        {
            foreach (var field in inputFields)
            {
                if (string.IsNullOrEmpty(field))
                    return false;

                bool result = DateTime.TryParse(field, out var value);
                if(result == false)
                    return false;
            }
            return true;
        }
    }
}
