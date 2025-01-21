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
        private readonly IRepository<JobAd> _repository;
        public JobController(IMapper mapper, IRepositoryFactory repositoryFactory)
        {
            _mapper = mapper;
            _repository = repositoryFactory.GetRepository<IRepository<JobAd>>();
        }

        [HttpPost]
        public async Task<IActionResult> CreateJobAd(JobAd jobAd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            jobAd.CreatedAt = DateTime.UtcNow;
            _repository.Insert(jobAd);

            return Ok();
        }
    }
}
