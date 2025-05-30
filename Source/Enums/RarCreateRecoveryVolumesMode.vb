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
Imports DevCase.RAR.Commands

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarCreateRecoveryVolumesMode "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies the mode for creating recovery volumes (.rev files) for a RAR archive.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Enum RarCreateRecoveryVolumesMode ' <TypeConverter(GetType(EnumDescriptionConverter))>

        ''' <summary>
        ''' Value <see cref="RarCreateRecoveryVolumesCommand.RecoveryVolumes"/> specifies 
        ''' a number of recovery volumes to create for the archive.
        ''' <para></para>
        ''' It must not be larger than tenfold amount of RAR volumes. Values exceeding the threshold are adjusted automatically.
        ''' </summary>
        NumberOfVolumes ' <Description("Number of Volumes")>

        ''' <summary>
        ''' Value <see cref="RarCreateRecoveryVolumesCommand.RecoveryVolumes"/> specifies a percentage.
        ''' <para></para>
        ''' The number of creating .rev files will be equal to this percentage taken from the total number of RAR volumes.
        ''' </summary>
        Percentage ' <Description("Percentage")>

    End Enum

End Namespace

#End Region
