using System;
using System.Collections.Generic;

namespace mERP.Data;

public partial class User
{
    public int Id { get; set; }

    public string? Usrid { get; set; }

    public string? Usrname { get; set; }

    public string? Usrpwd { get; set; }

    public string? Usremail { get; set; }

    public string? Usrcell { get; set; }

    public int? Usrsts { get; set; }

    public int? Usrrole { get; set; }

    public string? Usrpar { get; set; }

    public string? Usriniop { get; set; }
}
