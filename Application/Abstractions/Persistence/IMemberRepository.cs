using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Abstractions.Persistence;

public interface IMemberRepository
{
    Task AddAsync(Member member, CancellationToken ct = default);
    Task UpdateAsync(Member member, CancellationToken ct = default);
    Task RemoveAsync(Member member, CancellationToken ct = default);

    Task<Member?> GetAsync(Expression<Func<Member, bool>> expression, CancellationToken ct = default);
}
