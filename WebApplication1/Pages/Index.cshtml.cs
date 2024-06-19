using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.DirectoryServices.AccountManagement;

public class IndexModel : PageModel
{
    [BindProperty]
    public string sAMAccountName { get; set; }
    public bool UserExists { get; set; }

    public void OnGet(string userName)
    {
        if (!string.IsNullOrEmpty(userName))
        {
            ChangeUserExpirationDate(userName);
        }
    }

    public void OnPost()
    {
        if (!string.IsNullOrEmpty(sAMAccountName))
        {
            UserExists = CheckIfUserExists(sAMAccountName);
        }
    }

    private bool CheckIfUserExists(string sAMAccountName)
    {
        using (var context = new PrincipalContext(ContextType.Domain, "megacorp.local"))
        {
            using (var user = UserPrincipal.FindByIdentity(context, sAMAccountName))
            {
                return user != null;
            }
        }
    }

    private void ChangeUserExpirationDate(string sAMAccountName)
    {
        using (var context = new PrincipalContext(ContextType.Domain, "megacorp.local"))
        {
            using (var user = UserPrincipal.FindByIdentity(context, sAMAccountName))
            {
                if (user != null)
                {
                    user.AccountExpirationDate = DateTime.Now.AddDays(7);
                    user.Save();
                }
            }
        }
    }
}