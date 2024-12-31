using Application.Services;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class SharedResource
{

}
public class LanguageService
{
    private readonly IStringLocalizer _localizer;

    public LanguageService(IStringLocalizerFactory factory)
    {
        var type = typeof(SharedResource);
        var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
        _localizer = factory.Create(nameof(SharedResource), assemblyName.Name);
    }

    public string GetKey(string key)
    {
        var localizedString = _localizer[key];
        return localizedString.ResourceNotFound ? $"[{key}]" : localizedString.Value;
    }
}

