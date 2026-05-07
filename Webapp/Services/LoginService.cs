using Core.Model;


namespace WebApp.Service;

public class LoginService
{
    private readonly List<User> mUsers;

    public User? CurrentUser { get; private set; }
    public bool IsLoggedIn => CurrentUser is not null;
//Login user
    public LoginService()
    {
        mUsers = [
            new User { Name = "Træner", Password = "1234", Role = "træner" },
            new User { Name = "Spiller", Password = "2345", Role="Spiller"},
        ];
    }

    public User? ValidLogin(string name, string password)
    {
        var user = mUsers.FirstOrDefault(u => u.Name == name && u.Password == password);
        CurrentUser = user;           // husk hvem der er logged ind
        return user;
    }

    public bool CreateUser(string name, string password)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        if (mUsers.Any(u => string.Equals(u.Name, name, StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }

        mUsers.Add(new User { Name = name, Password = password, Role = "Normal" });
        return true;
    }

    public void SetCurrentUser(User user) => CurrentUser = user;

    public void Logout() => CurrentUser = null;
}