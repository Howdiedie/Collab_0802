using System;
using System.Collections.Generic;

namespace Collab.Models;

public partial class ProgramMember
{
    public int ProgramMemberId { get; set; }

    public int? ProgramId { get; set; }

    public int? MemberId { get; set; }

    public string? MemberState { get; set; }

    public virtual Member? Member { get; set; }

    public virtual Program? Program { get; set; }
}
