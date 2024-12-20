using AutoMapper;
using Jobify.Api.DTOs;
using Jobify.Api.Service;
using Jobify.BL.DALModels;
using Jobify.BL.Repositories;
using Jobify.BL.Security;
using Jobify.BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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
        private readonly IConfiguration _configuration;
        public UserController(IRepositoryFactory repositoryFactory, IPasswordHasher<User> passwordHasher, IMapper mapper, IConfiguration configuration)
        {
            _repository = repositoryFactory.GetRepository<IRepository<User>>(); ;
            _userTypeRepository = repositoryFactory.GetRepository<IRepository<UserType>>();
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _configuration = configuration;
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

        [HttpGet("{id}")]
        public ActionResult<UserDTO> Get(int id)
        {
            try
            {
                var user = _repository.GetById(id);
                if (user == null)
                {
                    return NotFound($"User with id {id} wasn't found.");
                }
                var userDto = _mapper.Map<UserDTO>(user);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserDTO userDto) 
        {
            var trimmedName = userDto.Name.Trim();
            var trimmedSurname = userDto.Surname.Trim();

            var existingUser = _repository
                .GetAll()
                .FirstOrDefault(u => u.Name.Equals(trimmedName, StringComparison.OrdinalIgnoreCase) &&
                                     u.Surname.Equals(trimmedSurname, StringComparison.OrdinalIgnoreCase));

            if (existingUser != null)
            {
                return BadRequest($"A user with the name {trimmedName} {trimmedSurname} already exists.");
            }

            var userType = _userTypeRepository.GetAll().FirstOrDefault(ut => ut.Name.Equals(userDto.UserType.Name));

            var passwordHash = _passwordHasher.HashPassword(null, userDto.Password);
            userDto.Password = passwordHash;

            var user = _mapper.Map<User>(userDto);
            user.UserType = userType;

            _repository.Insert(user);
            _repository.Save();

            return Ok("Creating new user is successful.");
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserDTO userDto)
        {
            try
            {
                var existingUser = _repository.GetById(id);

                if (existingUser == null)
                {
                    return NotFound($"User with id {id} wasn't found.");
                }

                existingUser.Name = userDto.Name;
                existingUser.Surname = userDto.Surname;
                existingUser.Mail = userDto.Mail;
                existingUser.IsEmailVerified = userDto.IsEmailVerified;
                existingUser.UserTypeId = userDto.UserType.Id;

                _repository.Update(existingUser);
                _repository.Save();

                var updatedUserDto = _mapper.Map<UserDTO>(existingUser);
                return Ok(updatedUserDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<UserDTO> Delete(int id)
        {
            try
            {
                var deletedUser = _repository.Delete(id);

                if (deletedUser == null)
                {
                    return NotFound($"User with id {id} wasn't found.");
                }
                var deletedUserDto = _mapper.Map<UserDTO>(deletedUser);

                return Ok(deletedUserDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("[action]")]
        public ActionResult Login(LoginDTO loginDto)
        {
            try
            {
                var genericLoginFail = "Incorrect username or password.";

                var trimmedEmail = loginDto.Email.Trim();
                var existingUser = _repository
                    .GetAll()
                    .FirstOrDefault(u => u.Mail.Equals(trimmedEmail, StringComparison.OrdinalIgnoreCase));

                if (existingUser == null)
                {
                    return Unauthorized(genericLoginFail);
                }

                var verificationResult = _passwordHasher.VerifyHashedPassword(null, existingUser.Password, loginDto.Password);
                if (verificationResult != PasswordVerificationResult.Success)
                {
                    return Unauthorized(genericLoginFail);
                }

                var jwtProvider = new JwtTokenProvider(_configuration);
                var token = jwtProvider.GenerateEmailVerificationToken(existingUser.Mail, existingUser.UserType.Name);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult<UserChangePasswordDto> ChangePassword(UserChangePasswordDto changePasswordDto)
        {
            try
            {
                var genericLoginFail = "Incorrect username or password.";

                var trimmedEmail = changePasswordDto.Email.Trim();
                var existingUser = _repository
                    .GetAll()
                    .FirstOrDefault(u => u.Mail.Equals(trimmedEmail, StringComparison.OrdinalIgnoreCase));

                if (existingUser == null)
                    return BadRequest($"Such user doesn't exist.");

                if (changePasswordDto.Password == changePasswordDto.ConfirmPassword)
                {
                    var passwordHash = _passwordHasher.HashPassword(null, changePasswordDto.Password);
                    existingUser.Password = passwordHash;
                    _repository.Update(existingUser);
                    _repository.Save();
                }

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendPasswordResetEmail(string email)
        {
            var user = _repository.GetAll().FirstOrDefault(u => u.Mail.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user == null)
                return BadRequest("User with this email doesn't exist.");

            var jwtProvider = new JwtTokenProvider(_configuration);
            var resetToken = jwtProvider.GenerateEmailVerificationToken(user.Mail, user.UserType.Name);

            var emailService = new EmailService(_configuration);
            string resetLink = $"http://localhost:5249/Auth/ChangePassword?token={resetToken}";
            string emailBody = $"<p>Click the link below to reset your password:</p><a href=\"{resetLink}\">{resetLink}</a>";

            await emailService.SendEmailAsync(email, "Password Reset Request", emailBody);

            return Ok("Password reset email sent.");
        }
    }
}
