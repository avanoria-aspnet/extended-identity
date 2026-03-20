namespace Application.Dtos.Members;

public record MembershipDetails
(
   string Id,
   string MembershipName,
   DateTime StartDate,
   DateTime EndDate
);

