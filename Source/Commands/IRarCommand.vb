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

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " IRarCommand "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR.Commands

    ''' <summary>
    ''' Represents the configuration options for RAR archive operations through <c>rar.exe</c> command-line executable file.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Interface IRarCommand

        ''' <summary>
        ''' Gets or sets the path to the <c>rar.exe</c> executable file (not <c>WinRAR.exe</c>).
        ''' </summary>
        ''' 
        ''' <value>
        ''' Path to the <c>rar.exe</c> executable file.
        ''' </value>
        Property RarExecPath As String

        ''' <summary>
        ''' Return the full <c>rar.exe</c> command-line arguments string based on the property values of this <see cref="IRarCommand"/>.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' A string with all configured <c>rar.exe</c> command-line arguments.
        ''' </returns>
        Function GetRarCommandlineArguments() As String

    End Interface

End Namespace

#End Region
