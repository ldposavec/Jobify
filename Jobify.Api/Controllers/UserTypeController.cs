using AutoMapper;
using Jobify.Api.DTOs;
using Jobify.Api.Service;
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
        private readonly IUserTypeAdapter _adapter;
        public UserTypeController(IRepositoryFactory repositoryFactory, IUserTypeAdapter adapter)
        {
            _repository = repositoryFactory.GetRepository<IRepository<UserType>>();
            _adapter = adapter;
        }

        // GET: api/<UserTypeController>
        [HttpGet]
        public ActionResult<IEnumerable<UserTypeDTO>> Get()
        {
            try
            {
                var userTypes = _repository.GetAll();
                var userTypeDtos = _adapter.ToDTOList(userTypes);
                return Ok(userTypeDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
