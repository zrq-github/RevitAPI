using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Reflection;

[ObfuscationAttribute(Exclude = true, ApplyToMembers = true)]
internal class RevitApplication : IExternalApplication
{
    public static string ApplicationId { get; } = "8402F7DD-5F72-4614-B98D-AAFF4F9EE639";

    public Result OnShutdown(UIControlledApplication application)
    {
        return Result.Succeeded;
    }

    public Result OnStartup(UIControlledApplication application)
    {
        return Result.Succeeded;
    }
}