using System;
using System.Collections.Generic;

namespace Collab.Models;

public partial class Intent
{
    public int IntentId { get; set; }

    public string? IntentName { get; set; }

    public int? MissionCountFinish { get; set; }

    public int? MissionCountTotal { get; set; }

    public int? ProgramId { get; set; }

    public virtual ICollection<Mission> Missions { get; set; } = new List<Mission>();

    public virtual Program? Program { get; set; }
}
