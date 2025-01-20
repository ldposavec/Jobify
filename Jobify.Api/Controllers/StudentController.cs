using AutoMapper;
using Jobify.Api.DTOs;
using Jobify.Api.Service;
using Jobify.BL.DALModels;
using Jobify.BL.Repositories;
using Jobify.BL.Security;
using Jobify.BL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jobify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IRepository<UserType> _userTypeRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public StudentController(IRepositoryFactory repositoryFactory, IPasswordHasher<User> passwordHasher, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = repositoryFactory.GetRepository<UserRepository>(); ;
            _userTypeRepository = repositoryFactory.GetRepository<IRepository<UserType>>();
            _studentRepository = repositoryFactory.GetRepository<IRepository<Student>>();
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterStudent(StudentRegistrationDTO registerDto)
        {
            var trimmedName = registerDto.User.Name.Trim();
            var trimmedSurname = registerDto.User.Surname.Trim();
            var studentUserType = _userTypeRepository.GetAll().FirstOrDefault(ut => ut.Name.Equals("Student", StringComparison.OrdinalIgnoreCase));

            var existingStudent = _userRepository
                .GetAll()
                .FirstOrDefault(u => u.Students.Any(e => e.Jmbag == registerDto.Jmbag));

            if (existingStudent != null)
            {
                return BadRequest($"A student with JMBAG {registerDto.Jmbag} already exists.");
            }

            var passwordHash = _passwordHasher.HashPassword(null, registerDto.User.Password);
            registerDto.User.Password = passwordHash;

            var student = _mapper.Map<Student>(registerDto);
            student.User.UserType = studentUserType;

            _userRepository.Insert(student.User);
            _studentRepository.Insert(student);
            _userRepository.Save();
            _studentRepository.Save();

            var jwtProvider = JwtTokenProvider.GetInstance(_configuration);
            string verificationToken = jwtProvider.GenerateEmailVerificationToken(registerDto.User.Mail, studentUserType.Name);

            var emailService = new EmailService(_configuration);
            string verificationLink = $"http://localhost:5249/Auth/VerifyEmail?token={verificationToken}";
            string emailBody = $"<p>Dear {registerDto.User.Name},</p>" +
                               $"<p>Please verify your email by clicking the link below:</p>" +
                               $"<a href=\"{verificationLink}\">{verificationLink}</a>" +
                               $"<p>Thank you for registering!</p>";

            await emailService.SendEmailAsync(registerDto.User.Mail, "Registration Verification", emailBody);

            return Ok("Registration successful! Please verify your email to activate your account.");
        }

        [HttpGet("verify-email")]
        public IActionResult VerifyEmail(string token)
        {
            var jwtProvider = JwtTokenProvider.GetInstance(_configuration);
            var email = jwtProvider.ValidateToken(token);

            if (email == null)
            {
                return BadRequest("Invalid or expired token.");
            }

            var user = _userRepository.GetAll().FirstOrDefault(u => u.Mail == email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.IsEmailVerified = true;
            _userRepository.Update(user);
            _userRepository.Save();

            return Ok("Email verified successfully.");
        }
    }
}
