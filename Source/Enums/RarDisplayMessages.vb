' ***********************************************************************
' Author   : ElektroStudios
' Modified : 17-May-2025
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

'Imports System.ComponentModel

'Imports DevCase.Runtime.TypeConverters
Imports DevCase.RAR.Commands

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarDisplayMessages "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies the display messages in <c>rar.exe</c> output.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    <Flags>
    Public Enum RarDisplayMessages ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' None.
        ''' <para></para>
        ''' When specified in <see cref="RarCommandBase.DisplayMessages"/> property, 
        ''' it activates quiet mode, where only error messages and questions are displayed.
        ''' </summary>
        None = 0 ' <Description("None")>

        ''' <summary>
        ''' The copyright string.
        ''' </summary>
        Copyright = 1 ' <Description("Copyright")>

        ''' <summary>
        ''' The "Done" string at the end of operation.
        ''' </summary>
        Done = 2 ' <Description("Done")>

        ''' <summary>
        ''' The archived names output when creating, testing or extracting an archive.
        ''' <para></para>
        ''' It disables directory creation messages when unpacking a file to non-existing directory.
        ''' It can affect some other archive processing commands as well. 
        ''' <para></para>
        ''' It does not hide other messages and total percentage indicator.
        ''' <para></para>
        ''' Minor visual artifacts are possible, such as percentage indicator overwriting few last characters of error messages.
        ''' </summary>
        ArchivedNames = 4 ' <Description("Archived Names")>

        ''' <summary>
        ''' The percentage indicator.
        ''' </summary>
        PercentageIndicator = 8 ' <Description("Percentage Indicator")>

        ''' <summary>
        ''' All display messages.
        ''' <para></para>
        ''' When specified in <see cref="RarCommandBase.DisplayMessages"/> property, does not disable any output messages.
        ''' </summary>
        All = RarDisplayMessages.Copyright Or RarDisplayMessages.Done Or
              RarDisplayMessages.ArchivedNames Or RarDisplayMessages.PercentageIndicator ' <Description("All")>

    End Enum

End Namespace

#End Region
