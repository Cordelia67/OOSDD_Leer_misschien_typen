namespace Typotrainer.Services;

// Simpele service om gebruikers te beheren tijdens de sessie
// Data wordt gewist wanneer de app sluit (geen database)
public static class UserService
{
    // Gebruikers database (blijft bestaan zolang app draait)
    private static readonly Dictionary<string, string> Users = new()
    {
        { "jan@voorbeeld.nl", "wachtwoord123" },
        { "marie@voorbeeld.nl", "demo1234" },
        { "admin@typotrainer.nl", "admin" }
    };

    // Laatst aangemaakt account (voor auto-fill)
    public static string? LastRegisteredEmail { get; private set; }
    public static string? LastRegisteredPassword { get; private set; }

    // Controleer of gebruiker bestaat
    public static bool UserExists(string email)
    {
        return Users.ContainsKey(email.ToLower());
    }

    // Valideer login
    public static bool ValidateLogin(string email, string password)
    {
        if (Users.TryGetValue(email.ToLower(), out string? correctPassword))
        {
            return password == correctPassword;
        }
        return false;
    }

    // Registreer nieuwe gebruiker
    public static bool RegisterUser(string email, string password)
    {
        if (UserExists(email))
        {
            return false; // Gebruiker bestaat al
        }

        Users[email.ToLower()] = password;

        // Onthoud voor auto-fill
        LastRegisteredEmail = email;
        LastRegisteredPassword = password;

        return true;
    }

    // Reset laatst geregistreerde gebruiker
    public static void ClearLastRegistered()
    {
        LastRegisteredEmail = null;
        LastRegisteredPassword = null;
    }
}