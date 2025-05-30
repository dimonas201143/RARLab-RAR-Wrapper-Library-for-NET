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

#Region " RarArchiveTimestampMode "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies what to do with the RAR archive date when modifying an archive.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Enum RarArchiveTimestampMode ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' Forces RAR to set the date of a modified RAR archive to the date of the newest file in the archive.
        ''' </summary>
        Update ' <Description("Update")>

        ''' <summary>
        ''' Keep original RAR archive date. 
        ''' <para></para>
        ''' Prevents RAR from changing the RAR archive date when modifying the archive.
        ''' </summary>
        Keep ' <Description("Keep")>

    End Enum

End Namespace

#End Region
