﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Authorization;

public record AppPermission(
    string Feature,
    string Action,
    string Gruop,
    string Description,
    bool IsBasic = false)
{
    public string Name { get; set; }
    public static string NameFor(string feature, string action) => $"Permissions. {feature}.{action}";
}

public class Permissions
{
    private static readonly AppPermission[] _all = new AppPermission[]
    {
        new (AppFeature.Users, AppAction.Create, AppRoleGroup.SystemAccess, "Create Users"),
        new (AppFeature.Users, AppAction.Read, AppRoleGroup.SystemAccess, "Read Users"),
        new (AppFeature.Users, AppAction.Update, AppRoleGroup.SystemAccess, "Update Users"),
        new (AppFeature.Users, AppAction.Delete, AppRoleGroup.SystemAccess, "Delete Users"),

        new (AppFeature.UserRoles, AppAction.Read, AppRoleGroup.SystemAccess, "Read User Roles"),
        new (AppFeature.UserRoles, AppAction.Update, AppRoleGroup.SystemAccess, "Update User Roles"),

        new (AppFeature.Roles, AppAction.Create, AppRoleGroup.SystemAccess, "Create Roles"),
        new (AppFeature.Roles, AppAction.Read, AppRoleGroup.SystemAccess, "Read Roles"),
        new (AppFeature.Roles, AppAction.Update, AppRoleGroup.SystemAccess, "Update Roles"),
        new (AppFeature.Roles, AppAction.Delete, AppRoleGroup.SystemAccess, "Delete Roles"),

        new (AppFeature.RoleClaims, AppAction.Read, AppRoleGroup.SystemAccess, "Read Role Claims/Permissions"),
        new (AppFeature.RoleClaims, AppAction.Update, AppRoleGroup.SystemAccess, "Update Role Claims/Permissions"),

        new (AppFeature.Employees, AppAction.Create, AppRoleGroup.ManagmentHierarchy, "Create Employees"),
        new (AppFeature.Employees, AppAction.Read, AppRoleGroup.ManagmentHierarchy, "Read Employees", IsBasic: true),
        new (AppFeature.Employees, AppAction.Update, AppRoleGroup.ManagmentHierarchy, "Update Employees"),
        new (AppFeature.Employees, AppAction.Delete, AppRoleGroup.ManagmentHierarchy, "Delete Employees")
    };

    public static IReadOnlyList<AppPermission> AdminPermissions { get; } =
        new ReadOnlyCollection<AppPermission>(_all.Where(p => !p.IsBasic).ToArray()); 
    
    public static IReadOnlyList<AppPermission> BasicPermissions { get; } =
        new ReadOnlyCollection<AppPermission>(_all.Where(p => p.IsBasic).ToArray());
}
