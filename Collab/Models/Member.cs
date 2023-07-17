using System;
using System.Collections.Generic;

namespace Collab.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public string? MemberAccount { get; set; }

    public string? MemberPassword { get; set; }

    public string? MemberName { get; set; }

    public string? MemberPhoto { get; set; }

    public virtual ICollection<Mission> Missions { get; set; } = new List<Mission>();

    public virtual ICollection<Notebook> Notebooks { get; set; } = new List<Notebook>();

    public virtual ICollection<Notify> Notifies { get; set; } = new List<Notify>();

    public virtual ICollection<ProgramMember> ProgramMembers { get; set; } = new List<ProgramMember>();
}
