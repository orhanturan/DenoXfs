using DenoXfs.Sim.Data;
using Microsoft.EntityFrameworkCore;

namespace DenoXfs.Sim.Idc.Cards;

/// <summary>Cards in the Card and CardChipResponse tables.</summary>
public sealed class EfCardCatalog : ICardCatalog
{
    private readonly IDbContextFactory<SimDbContext> _contexts;

    public EfCardCatalog(IDbContextFactory<SimDbContext> contexts)
    {
        _contexts = contexts;
    }

    public async Task<IReadOnlyList<IdcCard>> AllAsync(CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        var cards = await db.Cards.AsNoTracking().Include(c => c.ChipResponses).OrderBy(c => c.Name).ToListAsync(cancellationToken);
        return cards.Select(ToCard).ToList();
    }

    public async Task<IdcCard?> FindAsync(string name, CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        var card = await db.Cards.AsNoTracking().Include(c => c.ChipResponses).FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
        return card is null ? null : ToCard(card);
    }

    public async Task SaveAsync(IdcCard card, string? originalName, CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        var key = string.IsNullOrEmpty(originalName) ? card.Name : originalName;
        var entity = await db.Cards.Include(c => c.ChipResponses).FirstOrDefaultAsync(c => c.Name == key, cancellationToken);
        if (entity is null)
        {
            entity = new Card();
            db.Cards.Add(entity);
        }

        entity.Name = card.Name;
        entity.Track1 = card.Track1;
        entity.Track2 = card.Track2;
        entity.Track3 = card.Track3;
        entity.Atr = card.Atr;
        db.CardChipResponses.RemoveRange(entity.ChipResponses);
        entity.ChipResponses = card.ChipResponses.Select(r => new CardChipResponse { Command = r.Key, Response = r.Value }).ToList();
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(string name, CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        return await db.Cards.Where(c => c.Name == name).ExecuteDeleteAsync(cancellationToken) > 0;
    }

    private static IdcCard ToCard(Card card) => new(
        card.Name, card.Track1, card.Track2, card.Track3, card.Atr,
        card.ChipResponses.ToDictionary(r => r.Command, r => r.Response));
}
