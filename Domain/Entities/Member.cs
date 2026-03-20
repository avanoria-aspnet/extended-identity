namespace Domain.Entities;

public class Member
{
    public string Id { get; private set; } = null!;
    public string UserId { get; private set; } = null!;
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? ProfileImageUrl { get; private set; }

    private Member(string id, string userId, string? firstName, string? lastName, string? profileImageUrl)
    {
        Id = id;
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        ProfileImageUrl = profileImageUrl;
    }

    public static Member Create(string userId, string? firstName = null, string? lastName = null, string? profileImageUrl = null)
    => new(Guid.NewGuid().ToString(), userId, firstName, lastName, profileImageUrl);

    public static Member Create(string id, string userId, string? firstName = null, string? lastName = null, string? profileImageUrl = null) 
        => new(id, userId, firstName, lastName, profileImageUrl);

    public void UpdateProfileInformation(string firstName, string lastName, string? profileImageUrl = null)
    {
        FirstName = firstName;
        LastName = lastName;
        ProfileImageUrl = profileImageUrl;
    }
}


