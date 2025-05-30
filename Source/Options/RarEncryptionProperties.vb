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

Imports System.Diagnostics
Imports System.Security

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarEncryptionProperties "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Represents the encryption configuration for a RAR archive.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public NotInheritable Class RarEncryptionProperties : Inherits RarPassword

#Region " Properties "

        ''' <summary>
        ''' Gets a value indicating whether the file content should be encrypted.
        ''' <para></para>
        ''' This is always <see langword="True"/> for RAR encryption.
        ''' </summary>
        Public ReadOnly Property EncryptFileContent As Boolean = True

        ''' <summary>
        ''' Gets or sets a value indicating whether to encrypt the file headers.
        ''' <para></para>
        ''' If <see langword="True"/>, file names and other metadata will be encrypted along with the file content.
        ''' <para></para>
        ''' Default value is <see langword="False"/>. 
        ''' </summary>
        Public Property EncryptFileHeader As Boolean

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarEncryptionProperties"/> class.
        ''' </summary>
        ''' 
        ''' <param name="encryptFileHeader">
        ''' Indicates whether to encrypt file headers within the archive.
        ''' </param>
        ''' 
        ''' <param name="password">T
        ''' The password used for encryption.
        ''' </param>
        <DebuggerStepThrough>
        Public Sub New(encryptFileHeader As Boolean, password As SecureString)

            MyBase.New(password)
            Me.EncryptFileHeader = encryptFileHeader
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarEncryptionProperties"/> class.
        ''' </summary>
        ''' 
        ''' <param name="encryptFileHeader">
        ''' Indicates whether to encrypt file headers within the archive along with the file content, which is encrypted by default.
        ''' </param>
        ''' 
        ''' <param name="password">T
        ''' The password used for encryption.
        ''' </param>
        <DebuggerStepThrough>
        Public Sub New(encryptFileHeader As Boolean, password As String)

            MyBase.New(password)
            Me.EncryptFileHeader = encryptFileHeader
        End Sub

#End Region

    End Class

End Namespace

#End Region
