using System;
using System.Collections.Generic;

namespace pmes.entity.Entities;

public partial class Project
{
    public int? ParentProjectId { get; set; }

    public int DeveloperAccountId { get; set; }

    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = null!;

    public string? ProjectDescription { get; set; }

    public string? CountryIsoCode2 { get; set; }

    public string? CountryName { get; set; }

    public string? RegionId { get; set; }

    public Guid? StateId { get; set; }

    public Guid? CityId { get; set; }

    public Guid? PostalId { get; set; }

    public string? RegionName { get; set; }

    public string? StateName { get; set; }

    public string? CityName { get; set; }

    public string? PostalCode { get; set; }

    public string? TownBaranggay { get; set; }

    public string? AddressLine { get; set; }

    public string? FullAddress { get; set; }

    public string? Longitude { get; set; }

    public string? Latitude { get; set; }

    public string? AddressAdditionalInformation { get; set; }

    public string? MapAddress { get; set; }

    public Guid Guid { get; set; }

    public bool? IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsDeleted { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public virtual ICollection<AccessCodeProfile> AccessCodeProfiles { get; set; } = new List<AccessCodeProfile>();

    public virtual ICollection<Project> InverseParentProject { get; set; } = new List<Project>();

    public virtual Project? ParentProject { get; set; }

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
