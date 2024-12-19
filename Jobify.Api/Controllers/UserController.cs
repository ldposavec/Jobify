using AutoMapper;
using Jobify.Api.DTOs;
using Jobify.Api.Service;
using Jobify.BL.DALModels;
using Jobify.BL.Repositories;
using Jobify.BL.Security;
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
        //[Authorize(Roles = "user")]
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
            string resetLink = $"http://localhost:5148/Auth/ChangePassword?token={resetToken}";
            string emailBody = $"<p>Click the link below to reset your password:</p><a href=\"{resetLink}\">{resetLink}</a>";

            await emailService.SendEmailAsync(email, "Password Reset Request", emailBody);

            return Ok("Password reset email sent.");
        }
    }
}
