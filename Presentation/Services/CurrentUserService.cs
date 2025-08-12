using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Presentation.Services;

public class CurrentUserService()
{
    public Guid? UserId => Guid.Parse("47403b8e-b946-4e55-baca-f0cab5d3b2c4");
        
}