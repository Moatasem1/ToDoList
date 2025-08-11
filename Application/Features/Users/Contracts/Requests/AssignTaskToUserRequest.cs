using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Contracts.Requests;

public record AssignTaskToUserRequest(Guid TaskId, List<Guid> UserIds);

