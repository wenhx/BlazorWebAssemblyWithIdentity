using BlazorWebAssemblyWithIdentity.Shared;

namespace BlazorWebAssemblyWithIdentity.Client.Services;

public interface IUserService
{
    Task<IList<UserInfo>> GetUsersAsync(int page, int pageSize);
}
