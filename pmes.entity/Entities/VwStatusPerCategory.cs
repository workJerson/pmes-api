using System;
using System.Collections.Generic;

namespace pmes.entity.Entities;

public partial class VwStatusPerCategory
{
    public string StatusPerCategoryCode { get; set; } = null!;

    public string StatusCategoryCode { get; set; } = null!;

    public string StatusCategoryName { get; set; } = null!;

    public string StatusCode { get; set; } = null!;

    public string StatusName { get; set; } = null!;
}
