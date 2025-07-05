using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;

namespace ExpenseTracker.Identity.Models;

public class ApplicationUser : MongoIdentityUser<ObjectId>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public ApplicationUser() : base()
    {
    }

    public ApplicationUser(string userName, string email) : base(userName, email)
    {
    }
}