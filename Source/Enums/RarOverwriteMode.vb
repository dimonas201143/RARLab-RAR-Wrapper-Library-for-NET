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

#Region " RarOverwriteMode "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies the file overwrite mode when archiving.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Enum RarOverwriteMode ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' Ask before overwrite.
        ''' </summary>
        Ask ' <Description("Ask before overwrite.")>

        ''' <summary>
        ''' Overwrite all files.
        ''' </summary>
        Overwrite ' <Description("Overwrite all files.")>

        ''' <summary>
        ''' Skip existing files
        ''' </summary>
        Skip ' <Description("Skip existing files.")>

    End Enum


End Namespace

#End Region
