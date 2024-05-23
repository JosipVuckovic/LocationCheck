using System.Security.Principal;

namespace LocationCheck.Data.Models;

public class ApiUserIdentity : GenericIdentity
{
    public int UserId { get; init; }
    public ApiUserIdentity(string name, int userId) : base(name)
    {
        UserId = userId;
    }
}