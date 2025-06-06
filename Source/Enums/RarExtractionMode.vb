﻿' ***********************************************************************
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

#Region " RarExtractionMode "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies the mode for extracting a RAR archive.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Enum RarExtractionMode ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' Extract files excluding their path component, so all files are created in the same destination directory.
        ''' <para></para>
        ''' This equals to "e" <c>rar.exe</c> command.
        ''' </summary>
        ExtractWithoutPath ' <Description("Extract Without Path")>

        ''' <summary>
        ''' Extract files with full path. 
        ''' <para></para>
        ''' This equals to "x" <c>rar.exe</c> command.
        ''' </summary>
        ExtractWithPath ' <Description("Extract With Path")>

    End Enum

End Namespace

#End Region
