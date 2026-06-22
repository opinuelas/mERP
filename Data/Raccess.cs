using System;
using System.Collections.Generic;

namespace mERP.Data;

public partial class Raccess
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public string? Accctlr { get; set; }

    public string? Accval { get; set; }

    public string? Accmnu { get; set; }

    public string? Accdesc { get; set; }

    public string? Accpag { get; set; }

    public string? Accicon { get; set; }

    public string? Acctar { get; set; }

    public int? Accord { get; set; }

    public virtual ICollection<Raccess> InverseParent { get; set; } = new List<Raccess>();

    public virtual Raccess? Parent { get; set; }
}
