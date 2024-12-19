using AutoMapper;
using Jobify.Api.DTOs;
using Jobify.BL.DALModels;
using Jobify.BL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Jobify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _repository;
        private readonly IRepository<UserType> _userTypeRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        public UserController(IRepositoryFactory repositoryFactory, IPasswordHasher<User> passwordHasher, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<IRepository<User>>(); ;
            _userTypeRepository = repositoryFactory.GetRepository<IRepository<UserType>>();
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        // GET: api/<UserController>
        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> Get()
        {
            try
            {
                var users = _repository.GetAll();
                var usersDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
                return Ok(usersDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
