using SpredMedia.Authentication.Model.model;
using SpredMedia.CommonLibrary;

namespace SpredMedia.Authentication.Core.Interface
{
    public interface IEmailer
    {
        Task<ResponseDto<bool>> SendEmail(User userModel, string purpose, string template);
    }
}