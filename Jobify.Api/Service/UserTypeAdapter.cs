using Jobify.Api.DTOs;
using Jobify.BL.DALModels;

namespace Jobify.Api.Service
{
    public class UserTypeAdapter : IUserTypeAdapter
    {
        public UserType ToDomain(UserTypeDTO userTypeDTO)
        {
            return new UserType
            {
                Id = userTypeDTO.Id,
                Name = userTypeDTO.Name
            };
        }

        public UserTypeDTO ToDTO(UserType userType)
        {
            return new UserTypeDTO
            {
                Id = userType.Id,
                Name = userType.Name
            };
        }

        public IEnumerable<UserTypeDTO> ToDTOList(IEnumerable<UserType> userTypes)
        {
            return userTypes.Select(ToDTO).ToList();
        }
    }
}
