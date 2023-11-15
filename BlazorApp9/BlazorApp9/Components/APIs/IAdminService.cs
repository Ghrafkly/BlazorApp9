using BlazorApp9.Client;

namespace BlazorApp9.Components.APIs;
// WEQ
public interface IAdminService
{
    Task<List<UserInfo>> GetAllUsers();
    Task<UserInfo> GetUser(string id);

}
