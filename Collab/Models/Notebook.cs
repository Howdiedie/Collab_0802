using System;
using System.Collections.Generic;

namespace Collab.Models;

public partial class Notebook
{
    public int NotebookId { get; set; }

    public string? NotebookTitle { get; set; }

    public DateTime? NotebooAddDate { get; set; }

    public string? NotebookContent { get; set; }

    public int? ProgramId { get; set; }

    public int? MemberId { get; set; }

    public virtual Member? Member { get; set; }

    public virtual Program? Program { get; set; }
}
