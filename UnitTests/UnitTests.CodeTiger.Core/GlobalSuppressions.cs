using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("CodeTiger.Naming", "CT1702:Type names should use pascal casing.",
    Justification = "Unit test classes do not need to use pascal casing.")]
[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores",
    Justification = "Unit test classes do not need to use pascal casing.")]
[assembly: SuppressMessage("Design", "CA1034:Nested types should not be visible",
    Justification = "Unit test classes are allowed to be nested.")]
[assembly: SuppressMessage("CodeTiger.Naming", "CT1727:Methods returning a Task should be suffixed with 'Async'.",
    Justification = "Unit test method names do not need to have an Async suffix.")]
