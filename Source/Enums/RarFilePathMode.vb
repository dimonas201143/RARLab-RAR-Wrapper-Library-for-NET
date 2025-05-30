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

#Region " RarFilePathMode "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies how file paths are stored and restored when adding to or extracting from a RAR archive.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Enum RarFilePathMode ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' Use RAR default behavior.
        ''' <para></para>
        ''' If used when archiving or extracting, it defaults to: <see cref="RarFilePathMode.ExpandPathsToFullWithoutDriveLetter"/>.
        ''' </summary>
        [Default] ' <Description("Default")>

        ''' <summary>
        ''' If used when archiving, files are added to an archive without including the path information.
        ''' <para></para>
        ''' If used when extracting, archived paths are ignored for extracted files, 
        ''' so all files are created in the same destination directory.
        ''' <para></para>
        ''' Using this value is not suitable for common scenarios, 
        ''' as this could result in multiple files with the same name existing in the archive.
        ''' </summary>
        ExcludePathsFromFileNames ' <Description("Exclude Paths From FileNames")>

        ''' <summary>
        ''' If used when archiving, files added to an archive does not store the base directory path.
        ''' <para></para>
        ''' If used when extracting, files extracted from an archive does not extract the base directory path.
        ''' <para></para>
        ''' This value is ignored if path mask includes wildcards.
        ''' </summary>
        ExcludeBaseDirFromFileNames ' <Description("Exclude Base Dir From File Names")>

        ''' <summary>
        ''' If used when archiving, files added to an archive store full file paths, except the drive letter.
        ''' <para></para>
        ''' If used when extracting, files extracted from an archive extracts full file paths, except the drive letter.
        ''' </summary>
        ExpandPathsToFullWithoutDriveLetter ' <Description("Expand Paths to Full (Without Drive Letter)")>

        ''' <summary>
        ''' If used when archiving, files added to an archive store full file paths, including the drive letter.
        ''' <para></para>
        ''' If used when extracting, it will change underscores back to colons and create unpacked files 
        ''' in their original directories and disks. If the user also specified a destination path to extract, it will be ignored.
        ''' </summary>
        ExpandPathsToFullWithDriveLetter ' <Description("Expand Paths to Full (With Drive Letter)")>

    End Enum


End Namespace

#End Region
