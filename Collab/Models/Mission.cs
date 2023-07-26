using System;
using System.Collections.Generic;

namespace Collab.Models;

public partial class Mission
{
    public int MissionId { get; set; }

    public string? MissionName { get; set; }

    public DateTime? MisStartTime { get; set; }

    public DateTime? MisFinishTime { get; set; }

    public string MisState { get; set; } = null!;

    public string? MisDescribe { get; set; }

    public int? IntentId { get; set; }
    //public int IntentID { get; internal set; }
    public int? MemberId { get; set; }
    //public int MemberID { get; internal set; }
    public virtual Intent? Intent { get; set; }

    public virtual Member? Member { get; set; }


}
public class MissionViewModel {
    public string? MissionName { get; set; }
    public DateTime? MisStartTime { get; set; }
    public DateTime? MisFinishTime { get; set; }
    public string? MisState { get; set; }
}