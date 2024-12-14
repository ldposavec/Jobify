﻿using AutoMapper;
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
        // GET: api/<JobController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<JobController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<JobController>
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/<JobController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<JobController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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