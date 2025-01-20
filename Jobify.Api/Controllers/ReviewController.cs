using AutoMapper;
using Jobify.Api.DTOs;
using Jobify.BL.Builders;
using Jobify.BL.DALModels;
using Jobify.BL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Jobify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _repository;
        private readonly IRepository<Firm> _firmRepository;
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;

        public ReviewController(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<IReviewRepository>();
            _firmRepository = repositoryFactory.GetRepository<IRepository<Firm>>();
            _userRepository = repositoryFactory.GetRepository<UserRepository>();
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReviewDTO>> Get()
        {
            try
            {
                var reviews = _repository.GetAll();
                var reviewDtos = _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
                return Ok(reviewDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ReviewDTO> Get(int id)
        {
            try
            {
                var review = _repository.GetById(id);
                if (review == null)
                {
                    return NotFound($"Review with id {id} wasn't found.");
                }
                var result = _mapper.Map<ReviewDTO>(review);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<ReviewDTO> Post([FromBody] ReviewDTO value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var firm = _firmRepository.GetById(value.FirmId);
                if (firm == null)
                {
                    return NotFound($"Firm with id {value.FirmId} wasn't found.");
                }

                var user = _userRepository.GetById(value.UserId);
                if (user == null)
                {
                    return NotFound($"User with id {value.UserId} wasn't found.");
                }

                var existingReview = _repository.GetExistingReview(value.FirmId, value.UserId);
                if (existingReview != null)
                {
                    return Conflict("Such review already exists.");
                }

                var newReview = ReviewBuilder.Create()
                    .SetFirm(firm)
                    .SetUser(user)
                    .SetGrade(value.Grade)
                    .SetComment(value.Comment ?? string.Empty)
                    .Build();

                _repository.Insert(newReview);
                _repository.Save();

                value.Id = newReview.Id;

                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<ReviewDTO> Put(int id, [FromBody] ReviewDTO value)
        {
            try
            {
                var existingReview = _repository.GetById(id);
                if (existingReview == null)
                {
                    return NotFound($"Review with id {id} wasn't found.");
                }

                existingReview.Grade = value.Grade;
                existingReview.Comment = value.Comment;

                _repository.Update(existingReview);
                _repository.Save();

                value.Id = existingReview.Id;

                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<ReviewDTO> Delete(int id)
        {
            try
            {
                var deletedReview = _repository.Delete(id);
                if (deletedReview == null)
                {
                    return NotFound($"Review with id {id} wasn't found.");
                }

                var deletedReviewDto = _mapper.Map<ReviewDTO>(deletedReview);

                return Ok(deletedReviewDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("firm/{firmId}")]
        public ActionResult<IEnumerable<ReviewDTO>> GetReviewsByFirm(int firmId)
        {
            try
            {
                var reviews = _repository.GetReviewsByFirmId(firmId);
                if (!reviews.Any())
                {
                    return NotFound($"No reviews found for patient with id {firmId}.");
                }

                var reviewDTOs = reviews.Select(c => _mapper.Map<ReviewDTO>(c)).ToList();
                return Ok(reviewDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
