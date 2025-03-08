using System;
using System.Collections.Generic;

namespace spliteasy.Persistence.Models;

public partial class GroupMember
{
    public Guid GroupId { get; set; }

    public Guid UserId { get; set; }

    public DateTime? JoinedAt { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
