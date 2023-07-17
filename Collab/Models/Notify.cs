using System;
using System.Collections.Generic;

namespace Collab.Models;

public partial class Notify
{
    public int NotifyId { get; set; }

    public DateTime? NotifyDate { get; set; }

    public string? NotifyAction { get; set; }

    public string? NotifyType { get; set; }

    public string? ActionName { get; set; }

    public int? ProgramId { get; set; }

    public int? MemberId { get; set; }

    public string? MemberName { get; set; }

    public virtual Member? Member { get; set; }

    public virtual Program? Program { get; set; }
}
