' ***********************************************************************
' Author   : ElektroStudios
' Modified : 17-May-2025
' ***********************************************************************

' This source code is freely distributed as part of the "DevCase Class Library for .NET Developers".
'
' If you find this library useful, consider supporting my work by purchasing the full DevCase suite.
' It includes a powerful set of APIs covering a wide range of features and development topics.
'
' Visit the purchase page here:
' https://github.com/ElektroStudios/DevCase.github.io
'
' Thank you for your support!

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
