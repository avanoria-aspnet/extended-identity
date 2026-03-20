using Application.Abstractions.Identity;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Dtos.Identity;
using Application.Dtos.Members;
using Domain.Entities;

namespace Application.Services.Members;

public class MemberService(IIdentityService identityService, IMemberRepository repo) : IMemberService
{
    public async Task<MemberResult> CreateMemberAsync(RegisterMemberRequest request, CancellationToken ct = default)
    {
        if (request is null)
            return new MemberResult(false, ["request model must be provided"]);

        var existing = await identityService.FindExistingEmailAsync(request.Email);
        if (existing)
            return new MemberResult(false, ["An account with the same email already exists"]);

        var registerResult = await identityService.RegisterAsync(request.Email, request.Password, "Member");
        if (!registerResult.Succeeded)
            return new MemberResult(false, [.. registerResult.Errors, "Unable to register user account"]);

        if (string.IsNullOrWhiteSpace(registerResult.UserId))
            return new MemberResult(false, ["User Id is missing"]);

        var member = Member.Create(registerResult.UserId);
        await repo.AddAsync(member, ct);

        return new MemberResult(true, [], member.Id, member.UserId);
    }

    public async Task<MemberResult> DeleteMemberAsync(string id, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new MemberResult(false, ["Id is missing"]);

        var member = await repo.GetAsync(x => x.Id == id, ct);
        if (member is null)
            return new MemberResult(false, ["Member not found"]);

        await repo.RemoveAsync(member, ct);
        var deleted = await identityService.DeleteAsync(member.UserId);
        return deleted ? new MemberResult(deleted, []): new MemberResult(deleted, ["Unable to delete account"]);
    }

    public Task<MemberDetails?> GetMemberDetailsAsync(string id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<MemberDetails?> UpdateMemberDetailsAsync(UpdateMemberRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
