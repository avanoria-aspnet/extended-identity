using Application.Dtos.Members;

namespace Application.Abstractions.Services;

public interface IMemberService
{
    Task<MemberResult> CreateMemberAsync(RegisterMemberRequest request, CancellationToken ct = default);
    Task<MemberResult> DeleteMemberAsync(string id, CancellationToken ct = default);

    Task<MemberDetails?> UpdateMemberDetailsAsync(UpdateMemberRequest request, CancellationToken ct = default);
    Task<MemberDetails?> GetMemberDetailsAsync(string id, CancellationToken ct = default);
}
