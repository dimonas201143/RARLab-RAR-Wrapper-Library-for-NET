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

#Region " RarCompressionMode "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies the available compression modes for RAR archiving.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Enum RarCompressionMode ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' No compression is applied. Fastest operation but largest archive size.
        ''' </summary>
        Store = 0 ' <Description("Store")>

        ''' <summary>
        ''' Very fast compression with minimal processing time. Produces larger archive.
        ''' </summary>
        Fastest = 1 ' <Description("Fastest")>

        ''' <summary>
        ''' Fast compression with slightly better compression ratio than <see cref="RarCompressionMode.Fastest"/>.
        ''' </summary>
        Fast = 2 ' <Description("Fast")>

        ''' <summary>
        ''' Default. Balanced mode providing a good trade-off between speed and compression ratio.
        ''' </summary>
        Normal = 3 ' <Description("Normal")>

        ''' <summary>
        ''' Produces smaller archive with a moderate increase in processing time.
        ''' </summary>
        Good = 4 ' <Description("Good")>

        ''' <summary>
        ''' Best compression, resulting in the smallest possible archive size but the slowest processing time.
        ''' </summary>
        Best = 5 ' <Description("Best")>

    End Enum

End Namespace

#End Region
