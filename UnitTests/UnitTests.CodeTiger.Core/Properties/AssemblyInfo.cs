using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using Xunit;

[assembly: AssemblyTitle("UnitTests.CodeTiger.Core")]
[assembly: AssemblyDescription("Contains unit tests for the CodeTiger.Core assembly.")]
[assembly: AssemblyCompany("CodeTiger")]
[assembly: AssemblyProduct("CodeTigerLib")]
[assembly: AssemblyCopyright("Copyright © 2015 Chris Dahlberg")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguageAttribute("en-US")]

[assembly: CLSCompliant(false)]
[assembly: ComVisible(false)]

[assembly: AssemblyVersion("0.0.1.0")]
[assembly: AssemblyFileVersion("0.0.1.0")]
[assembly: AssemblyInformationalVersion("0.0.1.0")]

// Disable test parallelization due to intermittent timing issues with async-related tests
[assembly: CollectionBehavior(DisableTestParallelization = true)]