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
Imports DevCase.RAR.Commands

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarFileTimestamps "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies the timestamps of files in a RAR archive.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    <Flags>
    Public Enum RarFileTimestamps ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' No file date or time information.
        ''' <para></para>
        ''' When specified in <see cref="RarCreationCommand.FileTimestamps"/> property, 
        ''' does not preserves any file timestamps.
        ''' </summary>
        None = 0 ' <Description("None")>

        ''' <summary>
        ''' The file's creation time.
        ''' </summary>
        CreationTime = 1 ' <Description("Creation Time")>

        ''' <summary>
        ''' The file's modification (last write) time.
        ''' </summary>
        ModificationTime = 2 ' <Description("Modification Time")>

        ''' <summary>
        ''' The file's last access time.
        ''' </summary>
        LastAccessTime = 4 ' <Description("Last Access Time")>

        ''' <summary>
        ''' All available timestamps: creation, modification, and last access times.
        ''' <para></para>
        ''' When specified in <see cref="RarCreationCommand.FileTimestamps"/> property, 
        ''' preserves all available file timestamps.
        ''' </summary>
        All = RarFileTimestamps.CreationTime Or
              RarFileTimestamps.ModificationTime Or
              RarFileTimestamps.LastAccessTime ' <Description("All")>

    End Enum

End Namespace

#End Region
