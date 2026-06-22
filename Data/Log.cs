using System;
using System.Collections.Generic;

namespace mERP.Data;

public partial class Log
{
    public long Id { get; set; }

    public DateTime Ldate { get; set; }

    public string? Value { get; set; }

    public string? Userid { get; set; }

    public string? Clientip { get; set; }
}
