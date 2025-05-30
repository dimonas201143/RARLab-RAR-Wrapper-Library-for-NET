' ***********************************************************************
' Author   : ElektroStudios
' Modified : 19-May-2025
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel

'Imports DevCase.Core.IO.FileSystem
'Imports DevCase.Runtime.TypeConverters

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarVolumeSplitOptions "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Represents the options for splitting a RAR archive into multiple volumes (*.part1.rar, *.part2.rar, etc) 
    ''' of equal size.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Class RarVolumeSplitOptions

        ''' <summary>
        ''' Gets or sets the desired size for each volume.
        ''' <para></para>
        ''' If set to 0 (zero), automatic volume size detection will be used during archive creation.
        ''' <para></para>
        ''' Default value is 0 (zero).
        ''' </summary>
        ''' 
        ''' <value>
        ''' The desired size for each volume.
        ''' </value>
        <DisplayName("Volume Size")>
        <Description("Specifies the desired size for each volume. If set to 0, automatic volume size detection will be used during archive creation.")>
        Public Property VolumeSize As Long ' As FileSize ' <TypeConverter(GetType(FileSizeConverter))>

        ''' <summary>
        ''' Gets or sets the number of recovery volumes to create along with the archive.
        ''' <para></para>
        ''' Default value is 0 (zero).
        ''' </summary>
        ''' 
        ''' <value>
        ''' The number of recovery volumes to create along with the archive.
        ''' </value>
        <DisplayName("Recovery Volumes")>
        <Description("Specifies the number of recovery volumes to create along with the archive.")>
        Public Property NumberOfRecoveryVolumes As Short

    End Class

End Namespace

#End Region
