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

#Region " RarFileDeletionMode "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies the deletion behavior to apply to files when archiving.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Enum RarFileDeletionMode ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' The files will not be deleted when archiving.
        ''' </summary>
        DoNotDelete ' <Description("Do Not Delete")>

        ''' <summary>
        ''' The files will be sent to the recycle bin after archiving, allowing recovery if needed.
        ''' </summary>
        SendToRecycleBin ' <Description("Send to Recycle Bin")>

        ''' <summary>
        ''' The files will be permanently deleted after archiving.
        ''' </summary>
        PermanentDeletion ' <Description("Permanent Deletion")>

        ''' <summary>
        ''' Before deleting, the file data is overwritten by zero bytes to prevent recovery of deleted files, 
        ''' file is truncated and renamed to temporary name.
        ''' <para></para>
        ''' Please be aware that such approach is designed for usual hard disks, 
        ''' but may fail to overwrite the original file data on solid state disks, 
        ''' as result of SSD wear leveling technology and more complicated data addressing.
        ''' </summary>
        Wipe ' <Description("Wipe")>

    End Enum


End Namespace

#End Region
