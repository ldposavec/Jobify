using Jobify.Api.DTOs;
using Jobify.BL.DALModels;

namespace Jobify.Api.Service
{
    public interface IUserTypeAdapter
    {
        UserTypeDTO ToDTO(UserType userType);
        UserType ToDomain(UserTypeDTO userTypeDTO);
        IEnumerable<UserTypeDTO> ToDTOList(IEnumerable<UserType> userTypes);
    }
}
