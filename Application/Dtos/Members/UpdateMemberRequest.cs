namespace Application.Dtos.Members;

public record UpdateMemberRequest
(
    string Id,
    string FirstName,
    string LastName,
    string? PhoneNumber = null,
    string? ProfileImageUrl = null
);