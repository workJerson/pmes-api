using System;
using System.Collections.Generic;

namespace pmes.entity.Entities;

public partial class AccessCodeProfilePermission
{
    public int AccessPointsId { get; set; }

    public int AccessCodeProfileId { get; set; }

    public int AccessCodeProfilePermissionId { get; set; }

    public Guid Guid { get; set; }

    public bool? IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsDeleted { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public virtual AccessCodeProfile AccessCodeProfile { get; set; } = null!;
}
