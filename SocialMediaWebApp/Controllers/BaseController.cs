using Microsoft.AspNetCore.Mvc;
using SocialMediaWebApp.Core.IConfiguration;

namespace SocialMediaWebApp.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IHttpContextAccessor _httpContext;

        public BaseController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }
    }
}
