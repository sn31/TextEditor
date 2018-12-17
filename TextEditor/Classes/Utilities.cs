using System;
using Foundation;
using AppKit;
using System.IO;
namespace TextEditor.Classes
{
    public class Utilities
    {
        public Utilities()
        {
        }
        public static Boolean SaveDocument(NSWindow Window)
        {
            var EditorViewController = AppDelegate.FindViewController(Window.ContentViewController) as ViewController;
            NSAttributedStringDocumentAttributes attributes = new NSAttributedStringDocumentAttributes();
            attributes.DocumentType = NSDocumentType.RTF;
            NSError errors = new NSError();
            NSRange range = new NSRange(0, EditorViewController.Text.Length);
            NSUrl currentUrl = Window.RepresentedUrl;
            if (Window.RepresentedUrl != null)
            {
                var textSave = EditorViewController.TextStorage.GetFileWrapper(range, attributes, out errors);
                textSave.Write(currentUrl, NSFileWrapperWritingOptions.Atomic, currentUrl, out errors);
                return true;
            }
            else
            {
                var dlg = new NSSavePanel()
                {
                    Title = "Save Document"
                };
                dlg.BeginSheet(Window, (rslt) =>
                {
                    if (rslt == 1)
                    {
                        var path = dlg.Url.Path;
                        NSUrl newUrl = dlg.Url;
                        var textSave = EditorViewController.TextStorage.GetFileWrapper(range, attributes, out errors);
                        textSave.Write(newUrl, NSFileWrapperWritingOptions.Atomic, newUrl, out errors);
                        Window.DocumentEdited = false;
                        EditorViewController.View.Window.SetTitleWithRepresentedFilename(Path.GetFileName(path));
                        EditorViewController.View.Window.RepresentedUrl = dlg.Url;
                        EditorViewController.FilePath = path;
                        NSDocumentController.SharedDocumentController.NoteNewRecentDocumentURL(dlg.Url);
                    }
                });
                return false;
            }
        }
    }
}
