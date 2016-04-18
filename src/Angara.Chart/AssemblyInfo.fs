namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Angara.Chart")>]
[<assembly: AssemblyProductAttribute("Angara.Chart")>]
[<assembly: AssemblyDescriptionAttribute("Data visualization library for F#.")>]
[<assembly: AssemblyVersionAttribute("0.2.0")>]
[<assembly: AssemblyFileVersionAttribute("0.2.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.2.0"
    let [<Literal>] InformationalVersion = "0.2.0"
