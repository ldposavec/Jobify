using AutoMapper;
using Jobify.Api.DTOs;
using Jobify.BL.DALModels;
using Jobify.BL.Repositories;
using Jobify.BL.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Jobify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        private readonly OIBValidationService _oibValidationService;
        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserType> _userTypeRepository;
        private readonly IRepository<Firm> _firmRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        public EmployerController(OIBValidationService oibValidationService, IMapper mapper, IRepository<User> userRepository,
            IRepository<UserType> userTypeRepository, IRepository<Firm> firmRepository, IPasswordHasher<User> passwordHasher)
        {
            _oibValidationService = oibValidationService;
            _mapper = mapper;
            _userRepository = userRepository;
            _userTypeRepository = userTypeRepository;
            _firmRepository = firmRepository;
            _passwordHasher = passwordHasher;
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
            _userRepository.Save();

            return Ok("Employer registered successfully.");
        }
    }
}
