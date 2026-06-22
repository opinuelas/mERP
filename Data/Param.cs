using System;
using System.Collections.Generic;

namespace mERP.Data;

public partial class Param
{
    public decimal Id { get; set; }

    public string? Code { get; set; }

    public string? Alphac { get; set; }

    public string? Descrip { get; set; }

    public string? Value { get; set; }

    public DateTime? Udate { get; set; }

    public string? Userid { get; set; }
}
