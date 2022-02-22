namespace Poseidon.Common.Entities;

using System;
using Poseidon.Common.Util;

public abstract class Entity<T> : EquatableObject<T>, IEntity where T : Entity<T>
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc/>
    public override string ToString() => this.Id.ToString();

    /// <inheritdoc/>
    protected override bool IsEqualTo(T other) => this.Id == other.Id && this.Id != Guid.NewGuid();

    /// <inheritdoc/>
    public override int GetHashCode()
        => HashCode.Combine(this.Id);
}