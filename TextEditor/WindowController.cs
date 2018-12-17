// This file has been autogenerated from a class added in the UI designer.

using System;
using Foundation;
using AppKit;
using System.IO;


namespace TextEditor
{
	public partial class WindowController : NSWindowController //TextEditorWindowController
	{
		public WindowController (IntPtr handle) : base (handle)
		{
		}
        public override void WindowDidLoad()
        {
            base.WindowDidLoad();
            Window.Delegate = new EditorWindowDelegate(Window);
        }
        public void SaveDocument()
        {   
            var EditorViewController = AppDelegate.FindViewController(Window.ContentViewController) as ViewController;
            NSAttributedStringDocumentAttributes attributes = new NSAttributedStringDocumentAttributes();
            attributes.DocumentType = NSDocumentType.RTF;
            if (Window.RepresentedUrl != null )
            {
                NSError errors = new NSError();
                NSRange range = new NSRange(0, EditorViewController.Text.Length);
                var textSave = EditorViewController.TextStorage.GetFileWrapper(range, attributes,out errors);
                var path = Window.RepresentedUrl.Path;
                NSUrl currentUrl = Window.RepresentedUrl;
               
                textSave.Write(currentUrl, NSFileWrapperWritingOptions.Atomic, currentUrl, out errors);
                
                //File.WriteAllText(path, EditorViewController.Text);
            }
            else{
                var dlg = new NSSavePanel()
                {
                    Title = "Save Document"
                };
                dlg.BeginSheet(Window, (rslt) =>
                {
                    if (rslt == 1)
                    {
                        var path = dlg.Url.Path;
                        File.WriteAllText(path, EditorViewController.Text);
                        Window.DocumentEdited = false;
                        EditorViewController.View.Window.SetTitleWithRepresentedFilename(Path.GetFileName(path));
                        EditorViewController.View.Window.RepresentedUrl = dlg.Url;
                        EditorViewController.FilePath = path;

                        NSDocumentController.SharedDocumentController.NoteNewRecentDocumentURL(dlg.Url);
                    }
                });
            }
        }
        [Action("saveDocument:")]
        public void SaveDocument(NSObject sender)
        {
            SaveDocument();
        }
        [Action("saveDocumentAs:")]
        public void SaveDocumentAs(NSObject sender)
        {
            try {
                Window.RepresentedUrl = null;
            }
            catch {
                Console.WriteLine("something");
            }
            SaveDocument();
        }

        public NSPrintInfo PrintInfo { get; set; } = new NSPrintInfo();
        [Action("runPageLayout:")]
        public void RunPageLayout(NSObject sender)
        {

            // Define objects
            var dlg = new NSPageLayout();

            // Display dialog
            dlg.BeginSheet(PrintInfo, Window, () => {
                // Handle layout change
            });

        }

        //[Action("print:")]
        //public void PrintDocument(NSObject sender)
        //{
        //    var EditorViewController = AppDelegate.FindViewController(Window.ContentViewController) as ViewController;
        //   EditorViewController.PrintDocument(PrintInfo);
        //}
    }
}

