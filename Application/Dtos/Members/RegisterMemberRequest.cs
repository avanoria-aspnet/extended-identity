namespace Application.Dtos.Members;

public record RegisterMemberRequest
(
    string Email,
    string Password,

    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? ProfileImageUrl
);
