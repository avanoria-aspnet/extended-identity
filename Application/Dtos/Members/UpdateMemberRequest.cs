namespace Application.Dtos.Members;

public record UpdateMemberRequest
(
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? ProfileImageUrl
);