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
    public class FirmController : ControllerBase
    {
        private readonly IRepository<Firm> _repository;
        private readonly IMapper _mapper;
        public FirmController(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<IRepository<Firm>>();
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FirmSimplifiedDTO>> Get()
        {
            try
            {
                var firms = _repository.GetAll();
                var firmDtos = _mapper.Map<IEnumerable<FirmSimplifiedDTO>>(firms);

                return Ok(firmDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<FirmDTO> Get(int id)
        {
            var firm = _repository.GetById(id);
            if (firm == null)
            {
                return NotFound($"Firm with id {id} was not found.");
            }

            var dto = _mapper.Map<FirmDTO>(firm);
            if (firm.Picture != null)
            {
                dto.PictureBase64 = Convert.ToBase64String(firm.Picture);
            }
            return Ok(dto);
        }

        //[HttpPost("{id}/upload-picture")]
        //public IActionResult UploadPicture(int id, IFormFile image)
        //{
        //    var firm = _repository.GetById(id);
        //    if (firm == null)
        //    {
        //        return NotFound($"Firm with ID {id} not found.");
        //    }

        //    using (var memoryStream = new MemoryStream())
        //    {
        //        image.CopyTo(memoryStream);
        //        firm.Picture = memoryStream.ToArray(); 
        //    }

        //    _repository.Update(firm);
        //    _repository.Save();

        //    return Ok("Image uploaded successfully.");
        //}
    }
}
