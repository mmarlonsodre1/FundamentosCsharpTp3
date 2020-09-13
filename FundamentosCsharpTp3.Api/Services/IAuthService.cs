using FundamentosCsharpTp3.Models;
using FundamentosCsharpTp3.WebApplication.Repository;

namespace FundamentosCsharpTp3.Api.Services
{
    public interface IAuthService
    {
        UserRepository UserRepository { get; set; }

        public AuthenticationReturn Authenticate(User model);
    }
}
