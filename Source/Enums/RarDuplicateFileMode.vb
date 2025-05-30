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

#Region " RarDuplicateFileMode "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies how RAR processes identical (duplicate) files when archiving.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Enum RarDuplicateFileMode ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' Turns off identical file processing, so such files are compressed as usual files.
        ''' </summary>
        Disabled = 0 ' <Description("Disabled")>

        ''' <summary>
        ''' RAR analyzes the file contents before starting archiving. 
        ''' If several identical files are found, the first file in the set is saved as usual file 
        ''' and all following files are saved as references to this first file. 
        ''' <para></para>
        ''' It allows to reduce the archive size, but applies some restrictions to resulting archive. 
        ''' <para></para>
        ''' You must not delete or rename the first identical file in the archive after the archive was created, 
        ''' because it will make extraction of following files using it as a reference impossible. 
        ''' If you modify the first file, following files will also have the modified contents after extracting.
        ''' </summary>
        Enabled = 1 ' <Description("Enabled")>

        ''' <summary>
        ''' Similar to <see cref="RarDuplicateFileMode.Enabled"/>, with the only difference: 
        ''' it will display names of found identical files in <c>rar.exe</c> output before starting archiving.
        ''' </summary>
        EnabledAndShownInOutput = 2 ' <Description("Enabled and show in rar.exe output")>

    End Enum

End Namespace

#End Region
