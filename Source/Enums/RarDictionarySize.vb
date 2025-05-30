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

#Region " RarDictionarySize "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies the dictionary size used for RAR compression, expressed in kilobytes.
    ''' <para></para>
    ''' Larger dictionary sizes may improve compression ratio at the cost of memory usage.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Enum RarDictionarySize As Long ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' 1 KB (1.024 bytes)
        ''' </summary>
        Kb1 = 1 ' <Description("1 KB")>

        ''' <summary>
        ''' 2 KB (2.048 bytes)
        ''' </summary>
        Kb2 = 2 ' <Description("2 KB")>

        ''' <summary>
        ''' 4 KB (4.096 bytes)
        ''' </summary>
        Kb4 = 4 ' <Description("4 KB")>

        ''' <summary>
        ''' 8 KB (8.192 bytes)
        ''' </summary>
        Kb8 = 8 ' <Description("8 KB")>

        ''' <summary>
        ''' 16 KB (16.384 bytes)
        ''' </summary>
        Kb_16 = 16 ' <Description("16 KB")>

        ''' <summary>
        ''' 32 KB (32.768 bytes)
        ''' </summary>
        Kb_32 = 32 ' <Description("32 KB")>

        ''' <summary>
        ''' 64 KB (65.536 bytes)
        ''' </summary>
        Kb_64 = 64 ' <Description("64 KB")>

        ''' <summary>
        ''' 128 KB (131.072 bytes)
        ''' </summary>
        Kb__128 = 128 ' <Description("128 KB")>

        ''' <summary>
        ''' 256 KB (262.144 bytes)
        ''' </summary>
        Kb__256 = 256 ' <Description("256 KB")>

        ''' <summary>
        ''' 512 KB (524.288 bytes)
        ''' </summary>
        Kb__512 = 512 ' <Description("512 KB")>

        ''' <summary>
        ''' 1 MB (1.048.576 bytes)
        ''' </summary>
        Mb1 = 1024 ' <Description("1 MB")>

        ''' <summary>
        ''' 2 MB (2.097.152 bytes)
        ''' </summary>
        Mb2 = 2048 ' <Description("2 MB")>

        ''' <summary>
        ''' 4 MB (4.194.304 bytes)
        ''' </summary>
        Mb4 = 4096 ' <Description("4 MB")>

        ''' <summary>
        ''' 8 MB (8.388.608 bytes)
        ''' </summary>
        Mb8 = 8192 ' <Description("8 MB")>

        ''' <summary>
        ''' 16 MB (16.777.216 bytes)
        ''' </summary>
        Mb_16 = 16384 ' <Description("16 MB")>

        ''' <summary>
        ''' 32 MB (33.554.432 bytes)
        ''' </summary>
        Mb_32 = 32768 ' <Description("32 MB")>

        ''' <summary>
        ''' 64 MB (67.108.864 bytes)
        ''' </summary>
        Mb_64 = 65536 ' <Description("64 MB")>

        ''' <summary>
        ''' 128 MB (134.217.728 bytes)
        ''' </summary>
        Mb__128 = 131072 ' <Description("128 MB")>

        ''' <summary>
        ''' 256 MB (268.435.456 bytes)
        ''' </summary>
        Mb__256 = 262144 ' <Description("256 MB")>

        ''' <summary>
        ''' 512 MB (536.870.912 bytes)
        ''' </summary>
        Mb__512 = 524288 ' <Description("512 MB")>

        ''' <summary>
        ''' 1 GB (1.073.741.824 bytes)
        ''' </summary>
        Gb1 = 1048576 ' <Description("1 GB")>

        ''' <summary>
        ''' 2 GB (2.147.483.648 bytes)
        ''' </summary>
        Gb2 = 2097152 ' <Description("2 GB")>

        ''' <summary>
        ''' 3 GB (3.221.225.472 bytes)
        ''' </summary>
        Gb3 = 3145728 ' <Description("3 GB")>

        ''' <summary>
        ''' 4 GB (4.294.967.296 bytes)
        ''' </summary>
        Gb4 = 4194304 ' <Description("4 GB")>

        ''' <summary>
        ''' 5 GB (5.368.709.120 bytes)
        ''' </summary>
        Gb5 = 5242880 ' <Description("5 GB")>

        ''' <summary>
        ''' 6 GB (6.442.450.944 bytes)
        ''' </summary>
        Gb6 = 6291456 ' <Description("6 GB")>

        ''' <summary>
        ''' 7 GB (7.516.192.768 bytes)
        ''' </summary>
        Gb7 = 7340032 ' <Description("7 GB")>

        ''' <summary>
        ''' 8 GB (8.589.934.592 bytes)
        ''' </summary>
        Gb8 = 8388608 ' <Description("8 GB")>

    End Enum


End Namespace

#End Region
