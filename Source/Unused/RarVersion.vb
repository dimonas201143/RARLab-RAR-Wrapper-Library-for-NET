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

#Region " RarVersion "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies a RAR archive format version.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Enum RarVersion ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' RAR version 4. Legacy format with wide compatibility across older software.
        ''' </summary>
        RAR4 = 4 ' <Description("RARv4")>

        ''' <summary>
        ''' RAR version 5. Latest format offering better compression and features, but with reduced compatibility for third-party application.
        ''' </summary>
        RAR5 = 5 ' <Description("RARv5")>

    End Enum

End Namespace

#End Region
