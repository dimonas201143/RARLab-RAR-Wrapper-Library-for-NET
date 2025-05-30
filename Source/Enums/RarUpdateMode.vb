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

#Region " RarUpdateMode "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies the mode for updating files when adding to or extracting from a RAR archive.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Enum RarUpdateMode ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' When used for archiving, add files to archive as normally.
        ''' <para></para>
        ''' When used for extraction, extracts files from archive as normally.
        ''' </summary>
        Normal ' <Description("Normal")>

        ''' <summary>
        ''' When used for archiving, updates archived files older than the files to add. New files will not be added to the archive.
        ''' <para></para>
        ''' When used for extraction, only old files would be replaced with new versions extracted from the archive.
        ''' </summary>
        Freshen ' <Description("Freshen")>

        ''' <summary>
        ''' When used for archiving, adds files not yet in the archive and updates archived files that are older than files to add.
        ''' <para></para>
        ''' When used for extraction, only extract files not present on the disk and files newer than their copies on the disk.
        ''' </summary>
        Update ' <Description("Update")>

    End Enum

End Namespace

#End Region
