namespace BlazorApp9.Client.APIs;
// WEQ
public interface IAdminService
{
    Task<List<UserInfo>> GetAllUsers();
    Task<UserInfo> GetUser(string id);
}

