using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("csnex")]
[assembly: AssemblyDescription("CLI Neon Executor")]
// ToDo: Place the Git Describe SHA in the AssemblyConfiguration, ala src/version.cpp.in
#if DEBUG
    [assembly: AssemblyConfiguration("(Debug)")]
#else
    [assembly: AssemblyConfiguration("(Release)")]
#endif
[assembly: AssemblyCompany("https://github.com/ghewgill/neon-lang")]
[assembly: AssemblyProduct("Neon")]
[assembly: AssemblyCopyright("MIT License (MIT)")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("022357f8-1d0c-476f-92bb-b4fb255b6971")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
