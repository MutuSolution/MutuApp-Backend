﻿using System.Collections.ObjectModel;

namespace Common.Authorization;

public record AppPermission(string Feature, string Action, string Group, string Description, bool IsBasic = false)
{
    public string Name => NameFor(Feature, Action);

    public static string NameFor(string feature, string action)
    {
        return $"Permissions.{feature}.{action}";
    }
}

public class AppPermissions
{
    private static readonly AppPermission[] _all = new AppPermission[]
    {
            new(AppFeature.Users, AppAction.Create, AppRoleGroup.SystemAccess, "Create Users"),
            new(AppFeature.Users, AppAction.Update, AppRoleGroup.SystemAccess, "Update Users", IsBasic: true),
            new(AppFeature.Users, AppAction.Read, AppRoleGroup.SystemAccess, "Read Users", IsBasic: true),
            new(AppFeature.Users, AppAction.Delete, AppRoleGroup.SystemAccess, "Delete Users"),

            new(AppFeature.UserRoles, AppAction.Read, AppRoleGroup.SystemAccess, "Read User Roles"),
            new(AppFeature.UserRoles, AppAction.Update, AppRoleGroup.SystemAccess, "Update User Roles"),

            new(AppFeature.Roles, AppAction.Read, AppRoleGroup.SystemAccess, "Read Roles"),
            new(AppFeature.Roles, AppAction.Create, AppRoleGroup.SystemAccess, "Create Roles"),
            new(AppFeature.Roles, AppAction.Update, AppRoleGroup.SystemAccess, "Update Roles"),
            new(AppFeature.Roles, AppAction.Delete, AppRoleGroup.SystemAccess, "Delete Roles"),

            new(AppFeature.RoleClaims, AppAction.Read, AppRoleGroup.SystemAccess,
                "Read Role Claims/Permissions"),
            new(AppFeature.RoleClaims, AppAction.Update, AppRoleGroup.SystemAccess,
                "Update Role Claims/Permissions"),

            new(AppFeature.Links, AppAction.Read, AppRoleGroup.ManagementHierarchy,
                "Read Links", IsBasic: true),
            new(AppFeature.Links, AppAction.Create, AppRoleGroup.ManagementHierarchy,
                "Create Links", IsBasic: true),
            new(AppFeature.Links, AppAction.Update, AppRoleGroup.ManagementHierarchy,
                "Update Links", IsBasic: true),
            new(AppFeature.Links, AppAction.Delete, AppRoleGroup.ManagementHierarchy,
                "Delete Links", IsBasic: true)
    };

    public static IReadOnlyList<AppPermission> AdminPermissions { get; } =
       new ReadOnlyCollection<AppPermission>(_all).ToArray();
    //  new ReadOnlyCollection<AppPermission>(_all.Where(p => !p.IsBasic).ToArray()); //YG!

    public static IReadOnlyList<AppPermission> BasicPermissions { get; } =
        new ReadOnlyCollection<AppPermission>(_all.Where(p => p.IsBasic).ToArray());

    public static IReadOnlyList<AppPermission> AllPermissions { get; } =
        new ReadOnlyCollection<AppPermission>(_all);
}