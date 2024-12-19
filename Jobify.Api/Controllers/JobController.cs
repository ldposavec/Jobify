using AutoMapper;
using Jobify.BL.DALModels;
using Jobify.BL.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jobify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IMapper _mapper;
        private JobAdRepository _jobAdRepository;
        public JobController(IMapper mapper, JobAdRepository jobAdRepository)
        {
            _mapper = mapper;
            _jobAdRepository = jobAdRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateJobAd(JobAd jobAd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            jobAd.CreatedAt = DateTime.UtcNow;
            _jobAdRepository.Insert(jobAd);

            return Ok();
        }
    }
}
