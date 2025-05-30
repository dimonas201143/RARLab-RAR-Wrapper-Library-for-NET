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

#Region " RarListingMode "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies the mode for listing the contents of a RAR archive.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Enum RarListingMode ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' Lists archived file attributes, size, date, time and name, one file per line.
        ''' <para></para>
        ''' If file is encrypted, line starts from '*' character.
        ''' </summary>
        Normal ' <Description("Normal")>

        ''' <summary>
        ''' Lists bare file names with path, one per line, without any additional information.
        ''' </summary>
        NormalBare ' <Description("Normal (Bare)")>

        ''' <summary>
        ''' Lists the detailed file information in multiline mode. 
        ''' <para></para>
        ''' This information includes file checksum value, host OS, compression options and other parameters.
        ''' </summary>
        NormalTechnical ' <Description("Normal (Technical)")>

        ''' <summary>
        ''' Lists the detailed information not only for files, but also for service headers like NTFS streams or file security data.
        ''' </summary>
        NormalTechnicalAll ' <Description("Normal (Technical, All)")>

        ''' <summary>
        ''' Lists archived file attributes, size, packed size, compression ratio, date, time, checksum and name, one file per line. 
        ''' <para></para>
        ''' If file is encrypted, line starts from '*' character. 
        ''' <para></para>
        ''' For BLAKE2sp checksum only two first and one last symbol are displayed.
        ''' </summary>
        Verbose ' <Description("Verbose")>

        ''' <summary>
        ''' Lists bare file names with path, one per line, without any additional information.
        ''' </summary>
        VerboseBare ' <Description("Verbose (Bare)")>

        ''' <summary>
        ''' Lists the detailed file information in multiline mode. 
        ''' <para></para>
        ''' This information includes file checksum value, host OS, compression options and other parameters.
        ''' </summary>
        VerboseTechnical ' <Description("Verbose (Technical)")>

        ''' <summary>
        ''' Lists the detailed information not only for files, but also for service headers like NTFS streams or file security data.
        ''' </summary>
        VerboseTechnicalAll ' <Description("Verbose (Technical, All)")>

    End Enum

End Namespace

#End Region
