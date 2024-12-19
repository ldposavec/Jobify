using AutoMapper;
using Jobify.Api.DTOs;
using Jobify.Api.Service;
using Jobify.BL.DALModels;
using Jobify.BL.Repositories;
using Jobify.BL.Security;
using Jobify.BL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jobify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserType> _userTypeRepository;
        private readonly IRepository<Employer> _employerRepository;
        private readonly IRepository<Firm> _firmRepository;
        private readonly OIBValidationService _oibValidationService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public EmployerController(IRepositoryFactory repositoryFactory, OIBValidationService oibValidationService,   IPasswordHasher<User> passwordHasher, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = repositoryFactory.GetRepository<IRepository<User>>(); ;
            _userTypeRepository = repositoryFactory.GetRepository<IRepository<UserType>>();
            _employerRepository = repositoryFactory.GetRepository<IRepository<Employer>>();
            _firmRepository = repositoryFactory.GetRepository<IRepository<Firm>>();
            _oibValidationService = oibValidationService;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost("validate-oib")]
        public async Task<IActionResult> ValidateOIB([FromBody] string oib)
        {
            if (string.IsNullOrWhiteSpace(oib) || oib.Length != 11)
            {
                return BadRequest("Invalid OIB. Must be 11 digits.");
            }

            try
            {
                bool isValid = await _oibValidationService.ValidateOIBAsync(oib);
                if (isValid)
                {
                    return Ok("OIB is valid and active.");
                }
                else
                {
                    return NotFound("OIB does not exist or is inactive.");
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error validating OIB: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterEmployer(EmployerRegistrationDTO registerDto)
        {
            var trimmedName = registerDto.User.Name.Trim();
            var trimmedSurname = registerDto.User.Surname.Trim();

            var existingEmployer = _userRepository
                .GetAll()
                .FirstOrDefault(u => u.Name.Equals(trimmedName, StringComparison.OrdinalIgnoreCase) &&
                                     u.Surname.Equals(trimmedSurname, StringComparison.OrdinalIgnoreCase) &&
                                     u.Employers.Any(e => e.Firm.Oib == registerDto.Firm.Oib));

            if (existingEmployer != null)
            {
                return BadRequest($"An employer with the name {trimmedName} {trimmedSurname} in the specified firm already exists.");
            }

            bool isValid = await _oibValidationService.ValidateOIBAsync(registerDto.Firm.Oib);
            if (!isValid)
            {
                return BadRequest("OIB validation failed.");
            }

            var existingFirm = _firmRepository.GetAll().FirstOrDefault(f => f.Oib == registerDto.Firm.Oib);
            Firm firmToUse;
            if (existingFirm != null)
            {
                firmToUse = existingFirm;
            }
            else
            {
                firmToUse = _mapper.Map<Firm>(registerDto.Firm);
                _firmRepository.Insert(firmToUse);
            }

            var employerUserType = _userTypeRepository.GetAll().FirstOrDefault(ut => ut.Name.Equals("Employer", StringComparison.OrdinalIgnoreCase));

            var passwordHash = _passwordHasher.HashPassword(null, registerDto.User.Password);
            registerDto.User.Password = passwordHash;

            var employer = _mapper.Map<Employer>(registerDto);
            employer.Firm = firmToUse;  
            employer.User.UserType = employerUserType;  

            _userRepository.Insert(employer.User);
            _employerRepository.Insert(employer);
            _userRepository.Save();
            _employerRepository.Save();

            var jwtProvider = new JwtTokenProvider(_configuration);
            string verificationToken = jwtProvider.GenerateEmailVerificationToken(registerDto.User.Mail);
            Console.WriteLine(verificationToken);

            var emailService = new EmailService(_configuration);
            string verificationLink = $"http://localhost:5148/Account/VerifyEmail?token={verificationToken}";
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
            var jwtProvider = new JwtTokenProvider(_configuration);
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

        [HttpPost("generate-jwt")]
        public IActionResult GenerateTestJwt([FromBody] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email is required.");
            }

            try
            {
                var jwtProvider = new JwtTokenProvider(_configuration);
                string token = jwtProvider.GenerateEmailVerificationToken(email);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating token: {ex.Message}");
            }
        }

    }
}
