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

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarFindStringMode "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies the mode to perform a string search within the files in a RAR archive.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Enum RarFindStringMode ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' Case insensitive search (default).
        ''' </summary>
        CaseInsensitive ' <Description("Case Insensitive Search")>

        ''' <summary>
        ''' Case sensitive search.
        ''' </summary>
        CaseSensitive ' <Description("Case Sensitive Search")>

        ''' <summary>
        ''' Hexadecimal search.
        ''' </summary>
        Hexadecimal ' <Description("Hexadecimal Search")>

    End Enum

End Namespace

#End Region
