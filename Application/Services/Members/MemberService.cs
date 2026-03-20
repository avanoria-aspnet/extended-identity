using Application.Abstractions.Identity;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
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
        var created = await repo.AddAsync(member, ct);

        return created
            ? new MemberResult(true, [], member.Id, member.UserId)
            : new MemberResult(false, ["Unable to create member account"]);
    }

    public async Task<MemberResult> DeleteMemberAsync(string id, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new MemberResult(false, ["Id is missing"]);

        var member = await repo.GetAsync(x => x.UserId == id, ct);
        if (member is null)
            return new MemberResult(false, ["Member not found"]);

        await repo.RemoveAsync(member, ct);
        var deleted = await identityService.DeleteAsync(member.UserId);
        
        return deleted 
            ? new MemberResult(true, []) 
            : new MemberResult(false, ["Unable to delete account"]);
    }

    public async Task<MemberResult> UpdateMemberDetailsAsync(UpdateMemberRequest request, CancellationToken ct = default)
    {
        if (request is null)
            return new MemberResult(false, ["Request model must be provided"]);

        var member = await repo.GetAsync(x => x.Id == request.Id, ct);
        if (member is null)
            return new MemberResult(false, ["Member not fund"]);
        
        member.UpdateProfileInformation(request.FirstName, request.LastName, request.ProfileImageUrl);
        var memberUpdated = await repo.UpdateAsync(member, ct);

        if (!memberUpdated)
            return new MemberResult(false, ["Unable to update member details"]);

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            bool phoneNumberUpdated = await identityService.UpdatePhoneNumberAsync(member.Id, request.PhoneNumber);
            if (!phoneNumberUpdated)
                return new MemberResult(true, ["Member details updated but not phone number"]);
        }

        return new MemberResult(true, []);
    }

    public async Task<MemberDetailsResult> GetMemberDetailsAsync(string id, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new MemberDetailsResult(false, ["Id is missing"]);

        var member = await repo.GetAsync(x => x.Id == id, ct);
        if (member is null)
            return new MemberDetailsResult(false, ["Member not fund"]);

        var email = await identityService.GetEmailAsync(member.UserId);
        var phoneNumber = await identityService.GetPhoneNumberAsync(member.UserId);

        var memberDetails = new MemberDetails
        (
            member.Id,
            member.UserId,
            member.FirstName,
            member.LastName,
            email,
            phoneNumber,
            member.ProfileImageUrl
        );

        return new MemberDetailsResult(true, [], memberDetails);
    }


}
