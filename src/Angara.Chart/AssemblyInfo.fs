namespace AssemblyInfo

open System.Reflection
open System.Runtime.CompilerServices
open System.Runtime.InteropServices

module internal Const =
    [<Literal>]
    let Version = "0.1.0"

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[<assembly: AssemblyTitle("Angara.Chart")>]
[<assembly: AssemblyDescription("A framework to define charts of different plots such as markers, lines, heatmaps etc.")>]
[<assembly: AssemblyConfiguration("")>]
[<assembly: AssemblyCompany("Microsoft Research")>]
[<assembly: AssemblyProduct("Angara")>]
[<assembly: AssemblyCopyright("Copyright © 2016 Microsoft Research")>]
[<assembly: AssemblyTrademark("")>]
[<assembly: AssemblyCulture("")>]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[<assembly: ComVisible(false)>]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[<assembly: Guid("e20cb9ec-04ef-4e32-b085-adaeb4615d4e")>]

// Version information for an assembly consists of the following four values:
// 
//       Major Version
//       Minor Version 
//       Build Number
//       Revision
// 
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [<assembly: AssemblyVersion("1.0.*")>]
[<assembly: AssemblyVersion(Const.Version + ".0")>]
[<assembly: AssemblyFileVersion(Const.Version + ".0")>]

do
    ()