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
    public class UserTypeController : ControllerBase
    {
        private readonly IRepository<UserType> _repository;
        private readonly IMapper _mapper;
        public UserTypeController(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<IRepository<UserType>>(); ;
            _mapper = mapper;
        }

        // GET: api/<UserTypeController>
        [HttpGet]
        public ActionResult<IEnumerable<UserTypeDTO>> Get()
        {
            try
            {
                var userTypes = _repository.GetAll();
                var userTypeDtos = _mapper.Map<IEnumerable<UserTypeDTO>>(userTypes);
                return Ok(userTypeDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
