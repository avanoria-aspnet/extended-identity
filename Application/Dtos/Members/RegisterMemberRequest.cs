namespace Application.Dtos.Members;

public record RegisterMemberRequest
(
    string Email,
    string Password,

    string? FirstName = null,
    string? LastName = null,
    string? PhoneNumber = null,
    string? ProfileImageUrl = null
);
