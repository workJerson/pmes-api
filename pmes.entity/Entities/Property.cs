using System;
using System.Collections.Generic;

namespace pmes.entity.Entities;

public partial class Property
{
    public int ProjectId { get; set; }

    public int? ParentPropertyId { get; set; }

    public int PropertyId { get; set; }

    public string PropertyName { get; set; } = null!;

    public string? PropertyDescription { get; set; }

    public Guid Guid { get; set; }

    public bool? IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsDeleted { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public virtual ICollection<AccessPoint> AccessPoints { get; set; } = new List<AccessPoint>();

    public virtual ICollection<Property> InverseParentProperty { get; set; } = new List<Property>();

    public virtual Property? ParentProperty { get; set; }

    public virtual Project Project { get; set; } = null!;
}
