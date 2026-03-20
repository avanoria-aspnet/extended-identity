namespace Application.Dtos.Identity;

public record RegisterResult
(
    bool Succeeded,
    IReadOnlyCollection<string> Errors,
    string? UserId = null
);
