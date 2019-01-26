Written & Build using - Microsoft Visual Studio 2017, Version 15.8.1

Project wise dependencies:

Xvpn.BusinessLogic:-
CommonServiceLocator, version=2.0.4
Prism.Core, version=7.1.0.431
Prism.Wpf, version=7.1.0.431

Xvpn.UI :-
CommonServiceLocator, version=2.0.4
DynamicData, version=6.7.0.2529
Prism.Core, version=7.1.0.431
Prism.Unity, version=7.1.0.431
Prism.Wpf, version=7.1.0.431
Splat, version=5.1.4
System.Drawing.Primitives, version=4.3.0
System.ValueTuple, version=4.5.0
Unity.Abstractions, version=3.3.1
Unity.Container, version=5.8.11

Xvpn.Tests :-
Castle.Core, version=4.3.1
Moq, version=4.10.1
System.Runtime.CompilerServices.Unsafe, version=4.5.0
System.Threading.Tasks.Extensions, version=4.5.1
xunit, version=2.4.1
xunit.abstractions, version=2.0.3
xunit.analyzers, version=0.10.0
xunit.assert, version=2.4.1
xunit.core, version=2.4.1
xunit.extensibility.core, version=2.4.1
xunit.extensibility.execution, version=2.4.1
xunit.runner.visualstudio, version=2.4.1

For all dependencies the Target Framework is .Net 4.6.1

Build Steps :-
1. Clone repo.
2. Open XVPN.sln in Visual Studio 17.
3. Right-Click on the soultion and select "Manage Nuget Packages for Solution".
4. Click on "Restore" button on right hand side of Nuget tab window.
5. Once restore is complete, right-click on soultion and hit "Build Soultion".

Run Tests :-
1. From Main menu select Test -> Windows -> Test Explorer.
2. Test Expolrer pane is now visible on the left hand side, wait for a moment for the tests to load.
3. To run all test click Run All link.
