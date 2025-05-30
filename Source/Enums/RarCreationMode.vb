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

#Region " RarCreationMode "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies the mode for processing a RAR archive (create the archive, fresh or update).
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Enum RarCreationMode ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' Add files to archive. This will create a new archive.
        ''' <para></para>
        ''' This equals to "a" <c>rar.exe</c> command.
        ''' </summary>
        Add = RarUpdateMode.Normal ' <Description("Add")>

        ''' <summary>
        ''' Updates archived files older than the files to add. New files will not be added to the archive.
        ''' <para></para>
        ''' This equals to "f" <c>rar.exe</c> command.
        ''' </summary>
        Freshen = RarUpdateMode.Freshen ' <Description("Freshen")>

        ''' <summary>
        ''' Adds files not yet in the archive and updates archived files that are older than files to add.
        ''' <para></para>
        ''' This equals to "u" <c>rar.exe</c> command.
        ''' </summary>
        Update = RarUpdateMode.Update ' <Description("Update")>

    End Enum

End Namespace

#End Region
