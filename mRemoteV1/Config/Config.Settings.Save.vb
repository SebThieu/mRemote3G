Imports System.IO
Imports System.Xml
Imports mRemote3G.My
Imports mRemote3G.Tools
Imports mRemote3G.Forms

Namespace Config
    Namespace Settings
        Public Class Save
#Region "Public Methods"
            Public Shared Sub Save()
                Try
                    With frmMain
                        Dim windowPlacement As New Tools.WindowPlacement(frmMain)
                        If .WindowState = FormWindowState.Minimized And windowPlacement.RestoreToMaximized Then
                            .Opacity = 0
                            .WindowState = FormWindowState.Maximized
                        End If

                        MySettingsProperty.Settings.MainFormLocation = .Location
                        MySettingsProperty.Settings.MainFormSize = .Size

                        If Not .WindowState = FormWindowState.Normal Then
                            MySettingsProperty.Settings.MainFormRestoreLocation = .RestoreBounds.Location
                            MySettingsProperty.Settings.MainFormRestoreSize = .RestoreBounds.Size
                        End If

                        MySettingsProperty.Settings.MainFormState = .WindowState

                        MySettingsProperty.Settings.MainFormKiosk = frmMain.Fullscreen.Value

                        MySettingsProperty.Settings.FirstStart = False
                        MySettingsProperty.Settings.ResetPanels = False
                        MySettingsProperty.Settings.ResetToolbars = False
                        MySettingsProperty.Settings.NoReconnect = False

                        MySettingsProperty.Settings.ExtAppsTBLocation = .tsExternalTools.Location
                        If .tsExternalTools.Parent IsNot Nothing Then
                            MySettingsProperty.Settings.ExtAppsTBParentDock = .tsExternalTools.Parent.Dock.ToString
                        End If
                        MySettingsProperty.Settings.ExtAppsTBVisible = .tsExternalTools.Visible
                        MySettingsProperty.Settings.ExtAppsTBShowText = .cMenToolbarShowText.Checked

                        MySettingsProperty.Settings.QuickyTBLocation = .tsQuickConnect.Location
                        If .tsQuickConnect.Parent IsNot Nothing Then
                            MySettingsProperty.Settings.QuickyTBParentDock = .tsQuickConnect.Parent.Dock.ToString
                        End If
                        MySettingsProperty.Settings.QuickyTBVisible = .tsQuickConnect.Visible

                        MySettingsProperty.Settings.ConDefaultPassword = Security.Crypt.Encrypt(MySettingsProperty.Settings.ConDefaultPassword, App.Info.General.EncryptionKey)

                        MySettingsProperty.Settings.Save()
                    End With

                    SavePanelsToXML()
                    SaveExternalAppsToXML()
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Saving settings failed" & vbNewLine & vbNewLine & ex.ToString(), False)
                End Try
            End Sub

            Public Shared Sub SavePanelsToXML()
                Try
                    If Directory.Exists(App.Info.Settings.SettingsPath) = False Then
                        Directory.CreateDirectory(App.Info.Settings.SettingsPath)
                    End If

                    frmMain.pnlDock.SaveAsXml(App.Info.Settings.SettingsPath & "\" & App.Info.Settings.LayoutFileName)
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SavePanelsToXML failed" & vbNewLine & vbNewLine & ex.ToString(), False)
                End Try
            End Sub

            Public Shared Sub SaveExternalAppsToXML()
                Try
                    If Directory.Exists(App.Info.Settings.SettingsPath) = False Then
                        Directory.CreateDirectory(App.Info.Settings.SettingsPath)
                    End If

                    Dim xmlTextWriter As New XmlTextWriter(App.Info.Settings.SettingsPath & "\" & App.Info.Settings.ExtAppsFilesName, System.Text.Encoding.UTF8)
                    xmlTextWriter.Formatting = Formatting.Indented
                    xmlTextWriter.Indentation = 4

                    xmlTextWriter.WriteStartDocument()
                    xmlTextWriter.WriteStartElement("Apps")

                    For Each extA As Tools.ExternalTool In App.Runtime.ExternalTools
                        xmlTextWriter.WriteStartElement("App")
                        xmlTextWriter.WriteAttributeString("DisplayName", "", extA.DisplayName)
                        xmlTextWriter.WriteAttributeString("FileName", "", extA.FileName)
                        xmlTextWriter.WriteAttributeString("Arguments", "", extA.Arguments)
                        xmlTextWriter.WriteAttributeString("WaitForExit", "", extA.WaitForExit)
                        xmlTextWriter.WriteAttributeString("TryToIntegrate", "", extA.TryIntegrate)
                        xmlTextWriter.WriteEndElement()
                    Next

                    xmlTextWriter.WriteEndElement()
                    xmlTextWriter.WriteEndDocument()

                    xmlTextWriter.Close()
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SaveExternalAppsToXML failed" & vbNewLine & vbNewLine & ex.ToString(), False)
                End Try
            End Sub

            Private Sub New()
            End Sub
#End Region
        End Class
    End Namespace
End Namespace