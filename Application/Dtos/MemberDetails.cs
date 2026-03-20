namespace Application.Dtos;

public record MemberDetails
(
    string Id,
    string UserId,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string? ProfileImageUrl,
    MembershipDetails? Membership
);

