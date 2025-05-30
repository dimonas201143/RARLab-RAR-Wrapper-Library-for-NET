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

#Region " RarFileChecksumMode "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies the type of file checksum to use when archiving.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Enum RarFileChecksumMode ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' Use CRC-32 checksum, 32 bit length. 
        ''' <para></para>
        ''' While CRC-32 is suitable to detect most of unintentional data errors, 
        ''' it is not reliable enough to verify file data identity. 
        ''' In other words, if two files have the same CRC-32, it does not guarantee that file contents is the same.
        ''' </summary>
        CRC32 ' <Description("CRC-32")>

        ''' <summary>
        ''' Use BLAKE2sp hash, 256 bit length. 
        ''' <para></para>
        ''' BLAKE2sp error detection is more reliable than <see cref="RarFileChecksumMode.CRC32"/> checksum.
        ''' <para></para>
        ''' Being a cryptographically strong hash function, it practically guarantees that 
        ''' if two files have the same value of BLAKE2sp, their contents is the same. 
        ''' <para></para>
        ''' Since BLAKE2sp bit length is longer than CRC-32 checksum, resulting archive will be slightly larger.
        ''' </summary>
        BLAKE2sp ' <Description("BLAKE2sp")>

    End Enum

End Namespace

#End Region
