' ***********************************************************************
' Author   : ElektroStudios
' Modified : 17-May-2025
' ***********************************************************************

#Region " Usage Examples "

' Public Module Module1
' 
'     Public WithEvents RarExecutor As RarCommandExecutor
' 
'     Public Sub Main()
' 
'         Dim testCommand As New RarTestCommand("C:\Directory\*.rar") With {
'             .RarExecPath = ".\rar.exe",
'             .RarLicenseData = "(Your license key)",
'             .RecurseSubdirectories = False,
'             .Password = Nothing
'         }
' 
'         Module1.RarExecutor = New RarCommandExecutor(testCommand)
'         Dim exitcode As RarExitCode = RarExecutor.ExecuteRarAsync().Result
'     End Sub
' 
'     ''' <summary>
'     ''' Handles the <see cref="RarCommandExecutor.OutputDataReceived"/> event of the <see cref="RarExecutor"/> object.
'     ''' </summary>
'     ''' 
'     ''' <param name="sender">
'     ''' The source of the event, typically the underlying <c>rar.exe</c> <see cref="Process"/>.
'     ''' </param>
'     ''' 
'     ''' <param name="e">
'     ''' The <see cref="DataReceivedEventArgs"/> instance containing the event data.
'     ''' </param>
'     Private Sub OutputDataReceivedHandler(sender As Object, e As DataReceivedEventArgs) Handles RarExecutor.OutputDataReceived
' 
'         Console.WriteLine($"[Output] {Date.Now:yyyy-MM-dd HH:mm:ss} - {e.Data}")
'     End Sub
' 
'     ''' <summary>
'     ''' Handles the <see cref="RarCommandExecutor.ErrorDataReceived"/> event of the <see cref="RarExecutor"/> object.
'     ''' </summary>
'     ''' 
'     ''' <param name="sender">
'     ''' The source of the event, typically the underlying <c>rar.exe</c> <see cref="Process"/>.
'     ''' </param>
'     ''' 
'     ''' <param name="e">
'     ''' The <see cref="DataReceivedEventArgs"/> instance containing the event data.
'     ''' </param>
'     Private Sub ErrorDataReceivedHandler(sender As Object, e As DataReceivedEventArgs) Handles RarExecutor.ErrorDataReceived
' 
'         If e.Data IsNot Nothing Then
'             Console.WriteLine($"[Error] {Date.Now:yyyy-MM-dd HH:mm:ss} - {e.Data}")
'         End If
'     End Sub
' 
'     ''' <summary>
'     ''' Handles the <see cref="RarCommandExecutor.Exited"/> event of the <see cref="RarExecutor"/> object.
'     ''' </summary>
'     ''' 
'     ''' <param name="sender">
'     ''' The source of the event, typically the underlying <c>rar.exe</c> <see cref="Process"/>.
'     ''' </param>
'     ''' 
'     ''' <param name="e">
'     ''' The <see cref="EventArgs"/> instance containing the event data.
'     ''' </param>
'     Private Sub ExitedHandler(sender As Object, e As EventArgs) Handles RarExecutor.Exited
' 
'         Dim pr As Process = DirectCast(sender, Process)
'         Dim rarExitCode As RarExitCode = DirectCast(pr.ExitCode, RarExitCode)
'         Console.WriteLine($"[Exited] {Date.Now:yyyy-MM-dd HH:mm:ss} - rar.exe process has terminated with exit code {pr.ExitCode} ({rarExitCode})")
'     End Sub
' 
'     ''' <summary>
'     ''' Handles the <see cref="RarCommandExecutor.Disposed"/> event of the <see cref="RarExecutor"/> object.
'     ''' </summary>
'     ''' 
'     ''' <param name="sender">
'     ''' The source of the event, typically the underlying <c>rar.exe</c> <see cref="Process"/>.
'     ''' </param>
'     ''' 
'     ''' <param name="e">
'     ''' The <see cref="EventArgs"/> instance containing the event data.
'     ''' </param>
'     Private Sub DisposedHandler(sender As Object, e As EventArgs) Handles RarExecutor.Disposed
' 
'         Console.WriteLine($"[Disposed] {Date.Now:yyyy-MM-dd HH:mm:ss} - rar.exe process object have been disposed.")
'     End Sub
' 
' End Module

#End Region

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Diagnostics
Imports System.IO
Imports System.Text
Imports System.Threading.Tasks

Imports DevCase.RAR.Commands

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarCommandExecutor "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Represents an executor for the <c>rar.exe</c> process.
    ''' <para></para>
    ''' Manages execution, output handling, and lifecycle for the <c>rar.exe</c> process.
    ''' </summary>
    ''' 
    ''' <example> This is a code example.
    ''' <code language="VB">
    ''' Public Module Module1
    ''' 
    '''     Public WithEvents RarExecutor As RarCommandExecutor
    ''' 
    '''     Public Sub Main()
    ''' 
    '''         Dim testCommand As New RarTestCommand("C:\Directory\*.rar") With {
    '''             .RarExecPath = ".\rar.exe",
    '''             .RarLicenseData = "(Your license key)",
    '''             .RecurseSubdirectories = False,
    '''             .Password = Nothing
    '''         }
    ''' 
    '''         Module1.RarExecutor = New RarCommandExecutor(testCommand)
    '''         Dim exitcode As RarExitCode = RarExecutor.ExecuteRarAsync().Result
    '''     End Sub
    ''' 
    '''     ''' &lt;summary&gt;
    '''     ''' Handles the &lt;see cref="RarCommandExecutor.OutputDataReceived"/&gt; event of the &lt;see cref="RarExecutor"/&gt; object.
    '''     ''' &lt;/summary&gt;
    '''     ''' 
    '''     ''' &lt;param name="sender"&gt;
    '''     ''' The source of the event, typically the underlying &lt;c&gt;rar.exe&lt;/c&gt; &lt;see cref="Process"/&gt;.
    '''     ''' &lt;/param&gt;
    '''     ''' 
    '''     ''' &lt;param name="e"&gt;
    '''     ''' The &lt;see cref="DataReceivedEventArgs"/&gt; instance containing the event data.
    '''     ''' &lt;/param&gt;
    '''     Private Sub OutputDataReceivedHandler(sender As Object, e As DataReceivedEventArgs) Handles RarExecutor.OutputDataReceived
    ''' 
    '''         Console.WriteLine($"[Output] {Date.Now:yyyy-MM-dd HH:mm:ss} - {e.Data}")
    '''     End Sub
    ''' 
    '''     ''' &lt;summary&gt;
    '''     ''' Handles the &lt;see cref="RarCommandExecutor.ErrorDataReceived"/&gt; event of the &lt;see cref="RarExecutor"/&gt; object.
    '''     ''' &lt;/summary&gt;
    '''     ''' 
    '''     ''' &lt;param name="sender"&gt;
    '''     ''' The source of the event, typically the underlying &lt;c&gt;rar.exe&lt;/c&gt; &lt;see cref="Process"/&gt;.
    '''     ''' &lt;/param&gt;
    '''     ''' 
    '''     ''' &lt;param name="e"&gt;
    '''     ''' The &lt;see cref="DataReceivedEventArgs"/&gt; instance containing the event data.
    '''     ''' &lt;/param&gt;
    '''     Private Sub ErrorDataReceivedHandler(sender As Object, e As DataReceivedEventArgs) Handles RarExecutor.ErrorDataReceived
    ''' 
    '''         If e.Data IsNot Nothing Then
    '''             Console.WriteLine($"[Error] {Date.Now:yyyy-MM-dd HH:mm:ss} - {e.Data}")
    '''         End If
    '''     End Sub
    ''' 
    '''     ''' &lt;summary&gt;
    '''     ''' Handles the &lt;see cref="RarCommandExecutor.Exited"/&gt; event of the &lt;see cref="RarExecutor"/&gt; object.
    '''     ''' &lt;/summary&gt;
    '''     ''' 
    '''     ''' &lt;param name="sender"&gt;
    '''     ''' The source of the event, typically the underlying &lt;c&gt;rar.exe&lt;/c&gt; &lt;see cref="Process"/&gt;.
    '''     ''' &lt;/param&gt;
    '''     ''' 
    '''     ''' &lt;param name="e"&gt;
    '''     ''' The &lt;see cref="EventArgs"/&gt; instance containing the event data.
    '''     ''' &lt;/param&gt;
    '''     Private Sub ExitedHandler(sender As Object, e As EventArgs) Handles RarExecutor.Exited
    ''' 
    '''         Dim pr As Process = DirectCast(sender, Process)
    '''         Dim rarExitCode As RarExitCode = DirectCast(pr.ExitCode, RarExitCode)
    '''         Console.WriteLine($"[Exited] {Date.Now:yyyy-MM-dd HH:mm:ss} - rar.exe process has terminated with exit code {pr.ExitCode} ({rarExitCode})")
    '''     End Sub
    ''' 
    '''     ''' &lt;summary&gt;
    '''     ''' Handles the &lt;see cref="RarCommandExecutor.Disposed"/&gt; event of the &lt;see cref="RarExecutor"/&gt; object.
    '''     ''' &lt;/summary&gt;
    '''     ''' 
    '''     ''' &lt;param name="sender"&gt;
    '''     ''' The source of the event, typically the underlying &lt;c&gt;rar.exe&lt;/c&gt; &lt;see cref="Process"/&gt;.
    '''     ''' &lt;/param&gt;
    '''     ''' 
    '''     ''' &lt;param name="e"&gt;
    '''     ''' The &lt;see cref="EventArgs"/&gt; instance containing the event data.
    '''     ''' &lt;/param&gt;
    '''     Private Sub DisposedHandler(sender As Object, e As EventArgs) Handles RarExecutor.Disposed
    ''' 
    '''         Console.WriteLine($"[Disposed] {Date.Now:yyyy-MM-dd HH:mm:ss} - rar.exe process object have been disposed.")
    '''     End Sub
    ''' 
    ''' End Module
    ''' </code>
    ''' </example>
    Public Class RarCommandExecutor : Implements IDisposable

#Region " Private Fields "

        ''' <summary>
        ''' Gets the underlying <c>rar.exe</c> process.
        ''' </summary>
        Protected WithEvents RarProcess As Process

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the options used to configure the RAR execution.
        ''' </summary>
        Public Property Command As IRarCommand

#End Region

#Region " Events "

        ''' <summary>
        ''' Occurs when output data is received from the underlying <c>rar.exe</c> <see cref="Process"/> object.
        ''' </summary>
        Public Event OutputDataReceived As DataReceivedEventHandler

        ''' <summary>
        ''' Occurs when error data is received from the underlying <c>rar.exe</c> <see cref="Process"/> object.
        ''' </summary>
        Public Event ErrorDataReceived As DataReceivedEventHandler

        ''' <summary>
        ''' Occurs when the underlying <c>rar.exe</c> <see cref="Process"/> object has exited.
        ''' </summary>
        Public Event Exited As EventHandler

        ''' <summary>
        ''' Occurs when the underlying <c>rar.exe</c> <see cref="Process"/> object resources have been disposed.
        ''' </summary>
        Public Event Disposed As EventHandler

#End Region

#Region " Event Invocators "

        ''' <summary>
        ''' Raises the <see cref="RarCommandExecutor.OutputDataReceived"/> event.
        ''' </summary>
        '''
        ''' <param name="sender">
        ''' The source of the event, typically the <c>rar.exe</c> <see cref="Process"/> object.
        ''' </param>
        ''' 
        ''' <param name="e">
        ''' The <see cref="DataReceivedEventArgs"/> instance containing the event data.
        ''' </param>
        <DebuggerStepperBoundary>
        Private Sub RaiseOutputDataReceivedEvent(sender As Object, e As DataReceivedEventArgs)

            If Me.OutputDataReceivedEvent IsNot Nothing Then
                RaiseEvent OutputDataReceived(sender, e)
            End If
        End Sub

        ''' <summary>
        ''' Raises the <see cref="RarCommandExecutor.ErrorDataReceived"/> event.
        ''' </summary>
        '''
        ''' <param name="sender">
        ''' The source of the event, typically the <c>rar.exe</c> <see cref="Process"/> object.
        ''' </param>
        ''' 
        ''' <param name="e">
        ''' The <see cref="DataReceivedEventArgs"/> instance containing the event data.
        ''' </param>
        <DebuggerStepperBoundary>
        Private Sub RaiseErrorDataReceivedEvent(sender As Object, e As DataReceivedEventArgs)

            If Me.ErrorDataReceivedEvent IsNot Nothing Then
                RaiseEvent ErrorDataReceived(sender, e)
            End If
        End Sub

        ''' <summary>
        ''' Raises the <see cref="RarCommandExecutor.Exited"/> event.
        ''' </summary>
        '''
        ''' <param name="sender">
        ''' The source of the event, typically the <c>rar.exe</c> <see cref="Process"/> object.
        ''' </param>
        ''' 
        ''' <param name="e">
        ''' The <see cref="EventArgs"/> instance containing the event data.
        ''' </param>
        <DebuggerStepperBoundary>
        Private Sub RaiseExitedEvent(sender As Object, e As EventArgs)

            If Me.ExitedEvent IsNot Nothing Then
                RaiseEvent Exited(sender, e)
            End If
        End Sub

        ''' <summary>
        ''' Raises the <see cref="RarCommandExecutor.Disposed"/> event.
        ''' </summary>
        '''
        ''' <param name="sender">
        ''' The source of the event, typically the <c>rar.exe</c> <see cref="Process"/> object.
        ''' </param>
        ''' 
        ''' <param name="e">
        ''' The <see cref="EventArgs"/> instance containing the event data.
        ''' </param>
        <DebuggerStepperBoundary>
        Private Sub RaiseDisposedEvent(sender As Object, e As EventArgs)

            If Me.DisposedEvent IsNot Nothing Then
                RaiseEvent Disposed(sender, e)
            End If
        End Sub

#End Region

#Region " Event Handlers "

        ''' <summary>
        ''' Handles the <see cref="Process.OutputDataReceived"/> event of <see cref="RarCommandExecutor.RarProcess"/> object.
        ''' </summary>
        ''' 
        ''' <param name="sender">
        ''' The source of the event, typically the <c>rar.exe</c> <see cref="Process"/> object.
        ''' </param>
        ''' 
        ''' <param name="e">
        ''' The <see cref="DataReceivedEventArgs"/> instance containing the event data.
        ''' </param>
        <DebuggerStepperBoundary>
        Private Sub OutputDataReceivedHandler(sender As Object, e As DataReceivedEventArgs) Handles RarProcess.OutputDataReceived

            Me.RaiseOutputDataReceivedEvent(sender, e)
        End Sub

        ''' <summary>
        ''' Handles the <see cref="Process.ErrorDataReceived"/> event of <see cref="RarCommandExecutor.RarProcess"/> object.
        ''' </summary>
        ''' 
        ''' <param name="sender">
        ''' The source of the event, typically the <c>rar.exe</c> <see cref="Process"/> object.
        ''' </param>
        ''' 
        ''' <param name="e">
        ''' The <see cref="DataReceivedEventArgs"/> instance containing the event data.
        ''' </param>
        <DebuggerStepperBoundary>
        Private Sub ErrorDataReceivedHandler(sender As Object, e As DataReceivedEventArgs) Handles RarProcess.ErrorDataReceived

            Me.RaiseErrorDataReceivedEvent(sender, e)
        End Sub

        ''' <summary>
        ''' Handles the <see cref="Process.Exited"/> event of <see cref="RarCommandExecutor.RarProcess"/> object.
        ''' </summary>
        ''' 
        ''' <param name="sender">
        ''' The source of the event, typically the <c>rar.exe</c> <see cref="Process"/> object.
        ''' </param>
        ''' 
        ''' <param name="e">
        ''' The <see cref="EventArgs"/> instance containing the event data.
        ''' </param>
        <DebuggerStepperBoundary>
        Private Sub ExitedHandler(sender As Object, e As EventArgs) Handles RarProcess.Exited

            Me.RaiseExitedEvent(sender, e)
        End Sub

        ''' <summary>
        ''' Handles the <see cref="Process.Disposed"/> event of <see cref="RarCommandExecutor.RarProcess"/> object.
        ''' </summary>
        ''' 
        ''' <param name="sender">
        ''' The source of the event, typically the <c>rar.exe</c> <see cref="Process"/> object.
        ''' </param>
        ''' 
        ''' <param name="e">
        ''' The <see cref="EventArgs"/> instance containing the event data.
        ''' </param>
        <DebuggerStepThrough>
        Private Sub DisposedHandler(sender As Object, e As EventArgs) Handles RarProcess.Disposed

            Me.RaiseDisposedEvent(sender, e)
        End Sub

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarCommandExecutor"/> class.
        ''' </summary>
        ''' 
        ''' <param name="command">
        ''' An <see cref="IRarCommand"/> that provides the command-line configuration to run <c>rar.exe</c> process.
        ''' </param>
        <DebuggerStepThrough>
        Public Sub New(command As IRarCommand)

            Me.Command = command
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Asynchronously executes the <c>rar.exe</c> executable file 
        ''' with the command-line arguments configured through <see cref="RarCommandExecutor.Command"/>.
        ''' <para></para>
        ''' Redirects standard output asynchronously and waits for the process to exit.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' A <see cref="Task(Of RarExitCode)"/> representing the asynchronous operation, 
        ''' with the <see cref="RarExitCode"/> indicating the <c>rar.exe</c> process exit code.
        ''' </returns>
        <DebuggerStepThrough>
        Public Async Function ExecuteRarAsync() As Task(Of RarExitCode)

            Me.UpdateLicenseFile()

            Dim processStartInfo As New ProcessStartInfo With {
            .FileName = Me.Command.RarExecPath,
            .Arguments = Me.Command.GetRarCommandlineArguments(),
            .RedirectStandardOutput = True,
            .RedirectStandardError = True,
            .RedirectStandardInput = True,
            .UseShellExecute = False,
            .CreateNoWindow = True
        }

            Me.RarProcess?.Dispose()
            Me.RarProcess = New Process With {.StartInfo = processStartInfo, .EnableRaisingEvents = True}

            Try
                If Not Me.RarProcess.Start() Then
                    Throw New InvalidOperationException("rar.exe did not start: RarProcess.Start() returned False.")
                End If

            Catch ex As Exception
                Throw New InvalidOperationException("Failed to start rar.exe", ex)

            End Try

            Me.RarProcess.BeginOutputReadLine()
            Me.RarProcess.BeginErrorReadLine()

            Await Task.Run(Sub() Me.RarProcess.WaitForExit())

            Dim exitCode As Integer = Me.RarProcess.ExitCode
            Return DirectCast(exitCode, RarExitCode)
        End Function

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Writes the RAR license data to the <c>rarreg.key</c> file located in the <c>rar.exe</c> executable's directory,
        ''' but only if the license data is present and the license file does not yet exist.
        ''' <para></para>
        ''' Returns a <see cref="Boolean"/> value indicating whether the license file was successfully created.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' <see langword="True"/> if the license file was successfully created; 
        ''' <see langword="False"/> if the license file already exists, the license data is not available, 
        ''' the executable path is invalid, or an error occurred during the write operation.
        ''' </returns>
        ''' 
        ''' <exception cref="Exception">
        ''' Thrown if an unexpected error occurs while attempting to write the license file.
        ''' </exception>
        <DebuggerStepThrough>
        Private Function UpdateLicenseFile() As Boolean

            If Me.Command Is Nothing Then
                Return False
            End If

            Dim optionsBase As RarCommandBase = TryCast(Me.Command, RarCommandBase)
            If optionsBase Is Nothing Then
                Return False
            End If

            Dim licenseData As String = optionsBase.RarLicenseData
            If String.IsNullOrWhiteSpace(licenseData) Then
                Return False
            End If

            Try
                Dim rarExecPath As String = Me.Command.RarExecPath
                If String.IsNullOrWhiteSpace(rarExecPath) OrElse Not File.Exists(rarExecPath) Then
                    Return False
                End If

                Dim rarExecDir As String = Path.GetDirectoryName(rarExecPath)
                Dim rarregkeyFile As String = Path.Combine(rarExecDir, "rarreg.key")
                If Not File.Exists(rarregkeyFile) Then
                    File.WriteAllText(rarregkeyFile, licenseData, Encoding.UTF8)
                    Return True
                End If

            Catch ex As Exception
                Throw New Exception($"An error occurred while creating the RAR license file: {ex.Message}", ex)

            End Try

            Return False
        End Function

#End Region

#Region " IDisposable Implementation "

        ''' <summary>
        ''' Flag to prevent redundant calls to the <see cref="RarCommandExecutor.Dispose(Boolean)"/> method.
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
                    If Me.RarProcess IsNot Nothing Then

                        Try
                            If Not Me.RarProcess.HasExited Then
                                Me.RarProcess.Kill()
                            End If
                        Catch ex As Exception

                        End Try

                        Me.RarProcess.Dispose()
                        Me.RarProcess = Nothing
                    End If
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
                ' TODO: set large fields to null
                Me.disposedValue = True
            End If
        End Sub

        ''' <summary>
        ''' Releases all managed resources used by this <see cref="RarCommandExecutor"/>.
        ''' <para></para>
        ''' If the underlying <c>rar.exe</c> process is still running, it will be forcibly terminated.
        ''' </summary>
        ''' 
        ''' <remarks>
        ''' Call <see cref="Dispose"/> when you are finished using the <see cref="RarCommandExecutor"/> to ensure all
        ''' resources are released, including the external <c>rar.exe</c> process, if has yet not exited.
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
