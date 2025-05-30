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

Imports System.Diagnostics
Imports System.Security

Imports DevCase.Extensions

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarPassword "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Represents a RAR archive password for encryption or decryption.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Class RarPassword : Implements IDisposable

#Region " Properties "

        ''' <summary>
        ''' Gets the password used for encryption or decryption.
        ''' </summary>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Protected ReadOnly Property Password As SecureString

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Prevents a default instance of the <see cref="RarPassword"/> class from being created.
        ''' </summary>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarPassword"/> class.
        ''' </summary>
        ''' 
        ''' <param name="password">T
        ''' The password used for encryption or decryption.
        ''' </param>
        <DebuggerStepThrough>
        Public Sub New(password As SecureString)

            If password Is Nothing Then
                Throw New ArgumentNullException(NameOf(password), "Password can't be null")
            End If

            Me.Password = password.Copy()
            If Not Me.Password.IsReadOnly Then
                Me.Password.MakeReadOnly()
            End If
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarPassword"/> class.
        ''' </summary>
        ''' 
        ''' <param name="password">T
        ''' The password used for encryption or decryption.
        ''' </param>
        <DebuggerStepThrough>
        Public Sub New(password As String)

            If password Is Nothing Then
                Throw New ArgumentNullException(NameOf(password), "Password can't be null")
            End If

            Using ss As SecureString = StringExtensions.ToSecureString(password, [readOnly]:=False)
                Me.Password = ss.Copy()
                Me.Password.MakeReadOnly()
            End Using
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Retrieves the underlying password stored as a <see cref="SecureString"/> and converts it to a plain text string.
        ''' <para></para>
        ''' ⚠️ Use with caution. This method exposes sensitive information by converting a <see cref="SecureString"/> 
        ''' to a managed string in memory. 
        ''' Avoid keeping the returned string in memory longer than necessary.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' A <see cref="String"/> containing the password in plain text.
        ''' </returns>
        <DebuggerStepThrough>
        Protected Friend Function GetUnsafePassword() As String

            Return SecureStringExtensions.ToManagedString(Me.Password)
        End Function

#End Region

#Region " IDisposable Implementation "

        ''' <summary>
        ''' Flag to prevent redundant calls to the <see cref="RarPassword.Dispose(Boolean)"/> method.
        ''' </summary>
        Private disposedValue As Boolean

        ''' <summary>
        ''' Releases unmanaged and - optionally - managed resources.
        ''' </summary>
        ''' 
        ''' <param name="disposing">
        ''' Flag to prevent redundant calls.
        ''' </param>
        <DebuggerStepThrough>
        Private Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects)
                    Me.Password?.Dispose()
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
                ' TODO: set large fields to null
                Me.disposedValue = True
            End If
        End Sub

        ''' <summary>
        ''' Releases all managed resources of this <see cref="RarPassword"/>.
        ''' </summary>
        ''' 
        ''' <remarks>
        ''' Call <see cref="RarPassword.Dispose"/> when you have finished using the object.
        ''' </remarks>
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
            Me.Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub

#End Region

    End Class

End Namespace

#End Region
