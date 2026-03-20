namespace Application.Dtos.Members;

public record MemberResult
(
    bool Succeeded,
    IReadOnlyCollection<string> Errors,
    string? Id = null,
    string? UserId = null
);