namespace Application.Dtos.Members;

public record MemberDetailsResult
(
    bool Succeeded,
    IReadOnlyCollection<string> Errors,
    MemberDetails? MemberDetails = null
);