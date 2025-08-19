using DataTables.AspNet.Core;
using Domain.Common;
using Domain.Entities.User;

namespace Application.Features.Users.Specifications;

public class UserSpecfications : Specification<User>
{
    public UserSpecfications(Guid id)
    {
        Criteria = u => u.Id == id;
    }

    public UserSpecfications(List<Guid> ids)
    {
        Criteria = u => ids.Contains(u.Id);
    }

    public UserSpecfications(string email, Guid? exclude = null)
    {
        Criteria = u => u.Email == email && (exclude.HasValue ? u.Id != exclude : true);
    }

    public UserSpecfications(IDataTablesRequest request, bool doPagination = false)
    {

        var searchTerm = request.Search.Value?.ToLower().Trim();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            Criteria = u => (u.FirstName + " " + u.LastName.ToLower()).Contains(searchTerm) || u.Email.ToLower().Contains(searchTerm);

        }

        if (doPagination)
            ApplyPaging(request.Start, request.Length);
    }

    public UserSpecfications(string searchQuery, int pageSize, List<Guid> excludedUserIds)
    {
        searchQuery = searchQuery.Trim().ToLower();
        Criteria = u => (u.FirstName + " " + u.LastName).ToLower().Contains(searchQuery) &&
                        !excludedUserIds.Contains(u.Id);
        ApplyPaging(0, pageSize);
    }
}
