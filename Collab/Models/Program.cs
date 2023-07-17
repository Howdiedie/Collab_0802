using System;
using System.Collections.Generic;

namespace Collab.Models;

public partial class Program
{
    public int ProgramId { get; set; }

    public string? ProgramName { get; set; }

    public string? ProgramOverview { get; set; }

    public string? ProgramColor { get; set; }

    public virtual ICollection<Intent> Intents { get; set; } = new List<Intent>();

    public virtual ICollection<Notebook> Notebooks { get; set; } = new List<Notebook>();

    public virtual ICollection<Notify> Notifies { get; set; } = new List<Notify>();

    public virtual ICollection<ProgramLinkList> ProgramLinkLists { get; set; } = new List<ProgramLinkList>();

    public virtual ICollection<ProgramMember> ProgramMembers { get; set; } = new List<ProgramMember>();
}
