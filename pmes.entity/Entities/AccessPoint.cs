using System;
using System.Collections.Generic;

namespace pmes.entity.Entities;

public partial class AccessPoint
{
    public int PropertyId { get; set; }

    public int AccessPointsId { get; set; }

    public Guid AccessCode { get; set; }

    public string AccessPointsName { get; set; } = null!;

    public Guid Guid { get; set; }

    public bool? IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsDeleted { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public virtual ICollection<AccessCodeProfilePermission> AccessCodeProfilePermissions { get; set; } = new List<AccessCodeProfilePermission>();

    public virtual Property Property { get; set; } = null!;
}
